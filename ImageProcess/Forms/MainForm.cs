using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;
using ImageProcess.Filters;
using System.Reflection;
namespace ImageProcess.Forms;

public partial class MainForm : Form
{
    // Üzerinde çalıştığımız ana görüntü nesnesi.
    private AdvancedBitmap<Rgba32>? originalImage;
    private AdvancedBitmap<Rgba32> processedImage;   // Üzerinde işlem yapılacak kopya

    // Projede bulunan, uygulanabilir tüm filtrelerin listesi.
    private List<IFilter<Rgba32>> availableFilters = [];

    public MainForm()
    {
        InitializeComponent();
    }


    private void MainForm_Load(object sender, EventArgs e)
    {
        // 1. Projedeki tüm filtreleri otomatik olarak bul.
        DiscoverFilters();
        // 2. Bulunan bu filtrelerle "Filtreler" menüsünü dinamik olarak oluştur.
        BuildFilterMenu();
    }

    private void BuildFilterMenu()
    {
        var groupedFilters = availableFilters.GroupBy(f => f.Category);

        foreach (var group in groupedFilters)
        {
            var categoryMenuItem = new ToolStripMenuItem(group.Key);

            foreach (var filter in group)
            {
                var filterMenuItem = new ToolStripMenuItem(filter.Name);

                // ÖNEMLİ: Menü öğesinin 'Tag' özelliğine, filtrenin nesnesini atıyoruz.
                // Bu sayede hangi menüye tıklandığında hangi filtrenin çalışacağını bileceğiz.
                filterMenuItem.Tag = filter;

                // Tüm filtre menü öğelerini aynı 'Click' olayına bağlıyoruz.
                filterMenuItem.Click += filterMenuItem_Click;

                categoryMenuItem.DropDownItems.Add(filterMenuItem);
            }

            // Oluşturulan bu kategoriyi ana "Filtreler" menüsüne ekle.
            filtrelerToolStripMenuItem.DropDownItems.Add(categoryMenuItem);
        }
    }

    private async void filterMenuItem_Click(object? sender, EventArgs e)
    {

        if (originalImage == null)
        {
            MessageBox.Show("Lütfen önce bir resim açın.", "Uyarı");
            return;
        }

        var menuItem = sender as ToolStripMenuItem;
        var filter = menuItem.Tag as IOperation<Rgba32>;
        if (filter == null) return;

        // --- YENİ MANTIK BURADA BAŞLIYOR ---

        // 1. Her filtreye tıklamadan önce, çalışma tezgahını orijinal resmin temiz bir kopyasıyla sıfırla.
        processedImage?.Dispose();
        processedImage = (AdvancedBitmap<Rgba32>)originalImage.Clone();

        this.Cursor = Cursors.WaitCursor;

        // 2. Filtreyi, orijinal resme değil, onun taze kopyası olan _processedImage'e uygula.
        processedImage.Apply(filter);

        // 3. Sonucu _processedImage'den alıp göster.
        pictureBoxProcessed.Image = await Task.Run(() => (Bitmap)processedImage);

        this.Cursor = Cursors.Default;
    }

    private void DiscoverFilters()
    {
        availableFilters = [.. Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IFilter<Rgba32>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IFilter<Rgba32>) Activator.CreateInstance(t))];
    }

    private void açToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            originalImage?.Dispose(); // Varsa bir önceki orijinali temizle.
            processedImage?.Dispose(); // Varsa bir önceki işlenmişi temizle.

            // Orijinal resmi yükle.
            originalImage = (AdvancedBitmap<Rgba32>)new Bitmap(openFileDialog.FileName);

            // Orijinalin bir kopyasını işlenecek resim olarak ata.
            // Adım 6.1'de yazdığımız Clone() metodu burada işe yarıyor!
            processedImage = (AdvancedBitmap<Rgba32>)originalImage.Clone();

            pictureBoxOriginal.Image = (Bitmap)originalImage;
            // Başlangıçta işlenmiş resim de orijinalin aynısıdır.
            pictureBoxProcessed.Image = (Bitmap)processedImage;
        }
    }
    public static float[,] Laplacian3x3
    {
        get
        {
            return new float[,]
            { { -1, -1, -1,  },
                  { -1,  8, -1,  },
                  { -1, -1, -1,  }, };
        }
    }
    private void farklıKaydetToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (pictureBoxProcessed.Image == null)
        {
            MessageBox.Show("Kaydedilecek işlenmiş bir görüntü yok.", "Uyarı");
            return;
        }

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            pictureBoxProcessed.Image.Save(saveFileDialog.FileName);
        }
    }

    private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private async void laplacian3x3ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (originalImage == null)
        {
            MessageBox.Show("Lütfen önce bir resim açın.", "Uyarı");
            return;
        }





        //// 3. Sonucu _processedImage'den alıp göster.
        //pictureBoxProcessed.Image = await Task.Run(() => Exam.AddNoise(pictureBoxOriginal.Image));

        //this.Cursor = Cursors.Default;
    }
}
