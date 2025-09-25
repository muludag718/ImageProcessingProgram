using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;


namespace ImageProcess.Filters;

public struct GrayscaleFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "Gri Tonlama";
    public readonly string Category => "Renk Ayarları";

    public readonly void Execute(AdvancedBitmap<Rgba32> image) => image.ProcessRows(this);

    public void ProcessRow(Span<Rgba32> row)
    {
        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];

            // Parlaklık (Luminosity) formülünü kullanarak gri tonu hesapla.
            // double kullanarak daha hassas bir hesaplama yapıyoruz.
            double grayTone = (pixel.R * 0.299) + (pixel.G * 0.587) + (pixel.B * 0.114);

            // Hesaplanan değeri byte'a çevir.
            byte grayByte = (byte)grayTone;

            // Yeni pikselin tüm renk kanalları (R, G, B) aynı gri ton değerine sahip olur.
            // Alfa kanalını koruyoruz.
            row[x] = new Rgba32(grayByte, grayByte, grayByte, pixel.A);
        }
    }
}
