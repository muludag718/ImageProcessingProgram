using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;

namespace ImageProcess.Filters;

public struct BinaryFilter : IFilter<Rgba32>
{
    public readonly string Name => "Binary";
    public readonly string Category => "Renk Ayarları";

    public readonly void Execute(AdvancedBitmap<Rgba32> image)
    {

        int sum = 0;
        for (int y = 0; y < image.Height; y++)
        {
            foreach (var item in image.GetRowSpan(y))
            {
                sum += item.R + item.G + item.B;
            }
        }
        int average = Convert.ToInt32(sum / (image.Width * image.Height * 3));

        for (int y = 0; y < image.Height; y++)
        {
            var row = image.GetRowSpan(y);
            for (int x = 0; x < row.Length; x++)
            {
                var pixel = row[x];
                int newR = 0;
                int newG = 0;
                int newB = 0;

                if (pixel.R > average) newR = 255;
                if (pixel.G > average) newG = 255;
                if (pixel.B > average) newB = 255;

                int newSum = newR + newG + newB;

                if (newSum > 255) newSum = 255;

                row[x] = new Rgba32(
                    (byte)newSum,
                    (byte)newSum,
                    (byte)newSum,
                    pixel.A);

            }
        }


    }


}

