using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;

namespace ImageProcess.Filters;

public struct GamaFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "GamaFilter";

    public readonly string Category => "Filter";

    public void Execute(AdvancedBitmap<Rgba32> image)
    {
        image.ProcessRows(this);
    }

    public void ProcessRow(Span<Rgba32> row)
    {
        // Gamma değeri, parlaklık eğrisini ayarlar.
        // 1.0'dan küçük değerler (örneğin, 0.5) görüntüyü aydınlatır.
        // 1.0'dan büyük değerler (örneğin, 1.5) görüntüyü karartır.
        double gamma = 1.5;

        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];
            // Her renk kanalını 0-1 aralığına normalize et
            double R_norm = pixel.R / 255.0;
            double G_norm = pixel.G / 255.0;
            double B_norm = pixel.B / 255.0;

            // Gama dönüşümü formülünü uygula
            double R_new_norm = Math.Pow(R_norm, gamma);
            double G_new_norm = Math.Pow(G_norm, gamma);
            double B_new_norm = Math.Pow(B_norm, gamma);

            // Yeni değerleri tekrar 0-255 aralığına dönüştür
            int R = (int)(R_new_norm * 255);
            int G = (int)(G_new_norm * 255);
            int B = (int)(B_new_norm * 255);

            // Değerlerin 0-255 aralığında kalmasını sağla
            // Bu kontrol, işlem doğru yapıldığında genellikle gereksizdir.
            // Fakat olası yuvarlama hataları için eklenmesi güvenlidir.
            byte byteR = (byte)Math.Min(255, Math.Max(0, R));
            byte byteG = (byte)Math.Min(255, Math.Max(0, G));
            byte byteB = (byte)Math.Min(255, Math.Max(0, B));
           
            row[x] = new Rgba32(byteR, byteG, byteB, pixel.A);

        }
    }
}

