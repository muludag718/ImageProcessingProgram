using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;


namespace ImageProcess.Filters;

public class AutoContrastFilter : IFilter<Rgba32>
{
    public string Name => "AutoContrastFilter";

    public string Category => "Filter";

    public void Execute(ProcessContext<Rgba32> context)
    {
        var image = context.SourceImage;

        int minParlaklik = 255;
        int maxParlaklik = 0;


        for (int y = 0; y < image.Height; y++)
        {
            var row = image.GetRowSpan(y);
            for (int x = 0; x < row.Length; x++)
            {
                var pixel = row[x];

                var brightness = (int)((pixel.R + pixel.G + pixel.B) / 3.0);

                if (brightness < minParlaklik) minParlaklik = brightness;
                if (brightness > maxParlaklik) maxParlaklik = brightness;
            }
        }

        if (minParlaklik == maxParlaklik)
        {
            return;
        }

        double range = maxParlaklik - minParlaklik;

        for (int y = 0; y < image.Height; y++)
        {
            var row = image.GetRowSpan(y);
            for (int x = 0; x < row.Length; x++)
            {
                var pixel = row[x];

                double newR_double = ((pixel.R - minParlaklik) / range) * 255.0;
                double newG_double = ((pixel.G - minParlaklik) / range) * 255.0;
                double newB_double = ((pixel.B - minParlaklik) / range) * 255.0;

                byte newR = (byte)Math.Clamp(newR_double, 0, 255);
                byte newG = (byte)Math.Clamp(newG_double, 0, 255);
                byte newB = (byte)Math.Clamp(newB_double, 0, 255);

                row[x] = new Rgba32(newR, newG, newB, pixel.A);
            }
        }
    }
}
