using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcess.Core;

public class AdvancedBitmap<TPixel> : ICloneable, IDisposable where TPixel : struct, IPixel<TPixel>
{
    private IMemoryOwner<TPixel> pixelMemoryOwner;

    private Memory<TPixel> pixelMemory;

    private readonly Stack<IMemoryOwner<TPixel>> undoStack = new();

    private readonly Stack<IMemoryOwner<TPixel>> redoStack = new();

    private readonly List<IOperation<TPixel>> operationQueue = [];

    public int Width { get; private set; }

    // Görüntünün yüksekliği (sadece okunabilir).
    public int Height { get; private set; }

    // Kuyrukta bekleyen işlem olup olmadığını gösteren özellik.
    public bool HasPendingChanges => operationQueue.Count > 0;


    /// <summary>
    /// Belirtilen dosya yolundan bir görüntü yükleyerek yeni bir AdvancedBitmap nesnesi oluşturur.
    /// </summary>
    /// <param name="filePath">Görüntü dosyasının yolu.</param>
    public AdvancedBitmap(string filePath)
    {
        // 'using' ifadesi, standart Bitmap nesnesinin işimiz bittiğinde
        // bellekten düzgün bir şekilde temizlenmesini garanti eder.
        using var bmp = new Bitmap(filePath);

        // Asıl yükleme işini yapacak olan (henüz yazmadığımız)
        // ana metodumuzu çağırıyoruz.
        LoadFromBitmap(bmp);
    }

    /// <summary>
    /// Mevcut bir Bitmap nesnesinden veri alarak yeni bir AdvancedBitmap nesnesi oluşturur.
    /// </summary>
    /// <param name="sourceBitmap">Kaynak Bitmap nesnesi.</param>
    public AdvancedBitmap(Bitmap sourceBitmap)
    {
        // Gelen Bitmap nesnesini doğrudan ana yükleyici metodumuza iletiyoruz.
        LoadFromBitmap(sourceBitmap);
    }

    /// <summary>
    /// Belirtilen boyutlarda boş bir AdvancedBitmap tuvali oluşturan özel kurucu metot.
    /// Bu metot, sınıfın kendi içindeki operasyonlar için kullanılır.
    /// </summary>
    private AdvancedBitmap(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        // MemoryPool'dan pikselleri tutacak kadar bellek kirala.
        pixelMemoryOwner = MemoryPool<TPixel>.Shared.Rent(width * height);
        pixelMemory = pixelMemoryOwner.Memory;
    }

    /// <summary>
    /// Başka bir AdvancedBitmap nesnesinin derin kopyasını oluşturan özel kurucu metot.
    /// ICloneable arayüzünü uygularken kullanılır.
    /// </summary>
    private AdvancedBitmap(AdvancedBitmap<TPixel> source)
    {
        this.Width = source.Width;
        this.Height = source.Height;
        pixelMemoryOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height);
        pixelMemory = pixelMemoryOwner.Memory;
        // Kaynak görüntünün piksel verilerini, yeni oluşturulan bu nesnenin belleğine kopyala.
        source.pixelMemory.CopyTo(pixelMemory);
    }


    /// <summary>
    /// Standart bir Bitmap nesnesinin piksel verilerini kendi hızlı bellek alanımıza yükler.
    /// </summary>
    /// <param name="source">Kaynak Bitmap nesnesi.</param>
    private unsafe void LoadFromBitmap(Bitmap source)
    {
        this.Width = source.Width;
        this.Height = source.Height;

        // 2. Kendi piksellerimizi tutmak için MemoryPool'dan toplam piksel sayısı kadar bellek kirala.
        pixelMemoryOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height);
        pixelMemory = pixelMemoryOwner.Memory;

        var rect = new Rectangle(0, 0, Width, Height);

        BitmapData bmpData = source.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        try
        {
            var sourceSpan = new Span<Rgba32>(bmpData.Scan0.ToPointer(), Width * Height);

            var destSpan = pixelMemory.Span;

            int pixelCount = Width * Height;
            for (int i = 0; i < pixelCount; i++)
            {
                destSpan[i].FromRgba32(sourceSpan[i]);
            }
        }
        finally
        {
            source.UnlockBits(bmpData);
        }
    }


    /// <summary>
    /// Mevcut piksel verilerini, ekranda gösterilebilecek standart bir System.Drawing.Bitmap nesnesine dönüştürür.
    /// </summary>
    /// <returns>Piksel verilerini içeren yeni bir Bitmap nesnesi.</returns>
    public unsafe Bitmap ToBitmap()
    {
        if (HasPendingChanges) Execute();

        var bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

        var rect = new Rectangle(0, 0, Width, Height);

        var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        try
        {
            var sourceSpan = pixelMemory.Span;

            var destSpan = new Span<Rgba32>(bmpData.Scan0.ToPointer(), Width * Height);

            int pixelCount = this.Width * this.Height;
            for (int i = 0; i < pixelCount; i++)
            {
                destSpan[i] = sourceSpan[i].ToRgba32();
            }

        }
        finally
        {
            bmp.UnlockBits(bmpData);
        }

        return bmp;
    }

    #region Dönüştürme Operatörleri (Conversion Operators)

    /// <summary>
    /// AdvancedBitmap nesnesini standart System.Drawing.Bitmap nesnesine açıkça dönüştürür.
    /// Kullanım: Bitmap bmp = (Bitmap)myAdvancedBitmap;
    /// </summary>
    public static explicit operator Bitmap(AdvancedBitmap<TPixel> advancedBitmap)
    {
        return advancedBitmap.ToBitmap();
    }

    /// <summary>
    /// Standart System.Drawing.Bitmap nesnesini AdvancedBitmap nesnesine açıkça dönüştürür.
    /// Kullanım: AdvancedBitmap<Rgba32> adv = (AdvancedBitmap<Rgba32>)myBitmap;
    /// </summary>
    public static explicit operator AdvancedBitmap<TPixel>(Bitmap sourceBitmap)
    {
        // Bu operatör, Adım 2.2'de yazdığımız kurucu metodu çağırır.
        return new AdvancedBitmap<TPixel>(sourceBitmap);
    }

    #endregion

    /// <summary>
    /// Görüntünün belirtilen satırına, kopyalama yapmadan doğrudan erişim sağlayan bir Span döndürür.
    /// Bu, tüm piksel işleme operasyonlarının temel yapı taşıdır.
    /// </summary>
    /// <param name="rowIndex">Erişilmek istenen satırın indeksi (Y koordinatı).</param>
    /// <returns>Belirtilen satırı temsil eden bir Span<TPixel>.</returns>
    public Span<TPixel> GetRowSpan(int rowIndex)
    {
        return pixelMemory.Span.Slice(rowIndex * Width, Width);
    }

    /// <summary>
    /// Verilen bir 'processor' (işleyici) nesnesini, görüntünün her bir satırı üzerinde
    /// paralel olarak çalıştırır. Bu, en yüksek performanslı işleme yöntemidir.
    /// </summary>
    /// <typeparam name="TProcessor">IRowProcessor arayüzünü uygulayan struct tipindeki işleyici.</typeparam>
    /// <param name="processor">Her satır için ProcessRow metodu çağrılacak olan işleyici nesne.</param>
    public void ProcessRows<TProcessor>(TProcessor processor) where TProcessor : struct, IRowProcessor<TPixel>
    {
        Parallel.For(0, Height, y =>
        {
            var row = GetRowSpan(y);
            processor.ProcessRow(row);
        });
    }




    #region Değişecek

    /// <summary>
    /// Görüntüye, belirli bir kernel (matris) kullanarak evrişim uygular.
    /// Bu metot; bulanıklaştırma, netleştirme, kenar bulma gibi birçok filtrenin temelini oluşturur.
    /// </summary>
    /// <param name="kernel">Uygulanacak 2D evrişim matrisi (kernel).</param>
    public void Convolve(float[,] kernel)
    {

        int kernelHeigth = kernel.GetLength(0);
        int kernelWidth = kernel.GetLength(1);

        int radiusY = kernelHeigth / 2;
        int radiusX = kernelWidth / 2;

        var sourceBufferOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height);
        var sourceMemory = sourceBufferOwner.Memory;
        pixelMemory.CopyTo(sourceMemory);

        var sourceSpan = sourceMemory.Span;

        var destSpan = pixelMemory.Span;
        try
        {
            Parallel.For(0, Height, y =>
            {
                var sourceSpan = sourceMemory.Span;

                var destSpan = pixelMemory.Span;

                for (int x = 0; x < Width; x++)
                {
                    float sumR = 0, sumG = 0, sumB = 0;

                    for (int ky = 0; ky < kernelHeigth; ky++)
                    {

                        for (int kx = 0; kx < kernelWidth; kx++)
                        {
                            // Komşu pikselin koordinatını hesapla.
                            int pixelX = x + (kx - radiusX);
                            int pixelY = y + (ky - radiusX);

                            // Kenar kontrolü: Eğer kernel resmin dışına taşıyorsa, en yakın kenar pikselini kullan.
                            pixelX = Math.Max(0, Math.Min(Width - 1, pixelX));
                            pixelY = Math.Max(0, Math.Min(Height - 1, pixelY));

                            // Orijinal (kaynak) görüntüden komşu pikselin değerini ve kernel değerini al.
                            var sourcePixel = sourceSpan[pixelY * Width + pixelX].ToRgba32();
                            float kernelValue = kernel[kx, ky];

                            sumR += sourcePixel.R * kernelValue;
                            sumG += sourcePixel.G * kernelValue;
                            sumB += sourcePixel.B * kernelValue;
                        }
                    }
                    // Orijinal alfa değerini koru.
                    var originalAlpha = sourceSpan[y * Width + x].ToRgba32().A;
                    // Hesaplanan toplam değeri 0-255 arasına sıkıştırarak yeni pikseli oluştur.
                    var finalPixel = new Rgba32(
                        (byte)Math.Max(0, Math.Min(255, sumR)),
                        (byte)Math.Max(0, Math.Min(255, sumG)),
                        (byte)Math.Max(0, Math.Min(255, sumB)),
                        originalAlpha);
                    destSpan[y * Width + x].FromRgba32(finalPixel);

                }



            });
        }
        finally
        {
            // İşimiz bitti, kiraladığımız geçici belleği havuza iade ediyoruz.
            sourceBufferOwner.Dispose();
        }


    }
    #endregion


    /// <summary>
    /// Verilen bir operasyonu hemen uygulamak yerine işlem kuyruğuna ekler.
    /// Zincirleme kullanım için nesnenin kendisini döndürür.
    /// </summary>
    /// <param name="operation">Uygulanacak operasyon.</param>
    /// <returns>Operasyon eklenmiş olan aynı AdvancedBitmap nesnesi.</returns>
    public AdvancedBitmap<TPixel> Apply(IOperation<TPixel> operation)
    {
        operationQueue.Add(operation);
        return this;
    }


    /// <summary>
    /// İşlem kuyruğunda bekleyen tüm operasyonları sırayla uygular.
    /// Bu metot genellikle diğer metotlar tarafından dolaylı olarak çağrılır.
    /// </summary>
    private void Execute()
    {
        // Eğer kuyrukta bekleyen bir işlem yoksa hiçbir şey yapma.
        if (!HasPendingChanges) return;

        // Tüm operasyon zincirini tek bir 'Geri Al' adımı olarak kaydetmek için
        // işlemden önce mevcut durumu yığına it.
        PushToUndoStack();

        foreach (var oq in operationQueue)
        {
            oq.Execute(this);
        }
        operationQueue.Clear();


    }


    /// <summary>
    /// Görüntünün o anki durumunun bir kopyasını oluşturur ve Geri Al (Undo) yığınına ekler.
    /// Yeni bir işlem yapıldığında İleri Al (Redo) geçmişi temizlenir.
    /// </summary>
    public void PushToUndoStack()
    {
        // Bellek havuzundan, mevcut piksel verilerini kopyalamak için yeni bir bellek alanı kirala.
        var historyOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height);
        pixelMemory.CopyTo(historyOwner.Memory);

        // Kopyanın sahibini (fişini) geri alma yığınına it.
        undoStack.Push(historyOwner);

        // Yeni bir işlem zinciri başladığı için, artık ileri alınacak bir durum kalmamıştır.
        // İleri alma yığınındaki tüm bellek alanlarını havuza geri iade ederek yığını temizle.
        while (redoStack.Count > 0)
        {
            redoStack.Pop().Dispose();
        }
    }


    /// <summary>
    /// Geri alınacak bir işlem olup olmadığını belirtir.
    /// </summary>
    public bool CanUndo => undoStack.Count > 0;

    /// <summary>
    /// Son yapılan işlemi geri alır.
    /// </summary>
    public void Undo()
    {
        if (!CanUndo) return;

        // Mevcut durumu, ileri alabilmek için _redoStack'e it.
        redoStack.Push(pixelMemoryOwner);

        // _undoStack'ten bir önceki durumu çek ve onu aktif bellek yap.
        pixelMemoryOwner = undoStack.Pop();
        pixelMemory = pixelMemoryOwner.Memory;
    }


    /// <summary>
    /// İleri alınacak bir işlem olup olmadığını belirtir.
    /// </summary>
    public bool CanRedo => redoStack.Count > 0;

    /// <summary>
    /// Geri alınmış bir işlemi tekrar ileri alır.
    /// </summary>
    public void Redo()
    {
        if (!CanRedo) return;

        // Mevcut durumu, tekrar geri alabilmek için _undoStack'e it.
        undoStack.Push(pixelMemoryOwner);

        // _redoStack'ten bir sonraki durumu çek ve onu aktif bellek yap.
        pixelMemoryOwner = redoStack.Pop();
        pixelMemory = pixelMemoryOwner.Memory;
    }

    /// <summary>
    /// Mevcut AdvancedBitmap nesnesinin derin bir kopyasını (deep copy) oluşturur.
    /// Piksel verileri yeni bir bellek alanına kopyalanır, böylece klon ve orijinal birbirinden bağımsız olur.
    /// </summary>
    /// <returns>Bu nesnenin bir kopyası olan yeni bir nesne.</returns>
    public object Clone()
    {
        return new AdvancedBitmap<TPixel>(this);
    }


    private bool disposed = false;

    /// <summary>
    /// Sınıf tarafından kiralanan tüm bellek kaynaklarını MemoryPool'a iade eder.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        // Bu nesne bizim tarafımızdan temizlendiği için, Çöp Toplayıcı'nın
        // onu ayrıca temizlemeye çalışmasına gerek olmadığını bildiriyoruz. Bu bir optimizasyondur.
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }
        if (disposing)
        {
            // Kiralanan tüm bellekleri havuza iade et.
            pixelMemoryOwner?.Dispose();

            while (undoStack.Count > 0)
            {
                undoStack.Pop().Dispose();
            }
            while (redoStack.Count > 0)
            {
                redoStack.Pop().Dispose();
            }

        }

        disposed = true;
    }
}
