using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;


namespace ImageProcess.Filters;

public struct InvertFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "Renkleri Ters Çevir";
    public readonly string Category => "Renk Ayarları";


    /// <summary>
    /// Bu operasyonun ana giriş noktasıdır.
    /// Görüntüye, bu filtrenin kendisini bir satır işlemcisi olarak uygular.
    /// </summary>
    public readonly void Execute(AdvancedBitmap<Rgba32> image)
    {
        // AdvancedBitmap'in paralel motoruna, "işçi" olarak bu struct'ın kendisini ('this') veriyoruz.
        // 'ref this', struct'ın kopyalanmasını engeller.
        image.ProcessRows(this);
    }

    public readonly void ProcessRow(Span<Rgba32> row)
    {
        // Satırdaki her bir piksel için döngü
        for (int i = 0; i < row.Length; i++)
        {
            var pixel = row[i];

            byte newR = (byte)(255 - pixel.R);
            byte newG = (byte)(255 - pixel.G);
            byte newB = (byte)(255 - pixel.B);
            row[i] = new Rgba32(newR, newG, newB, pixel.A);

        }
    }
}
