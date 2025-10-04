using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;

namespace ImageProcess.Filters.BinaryOperations;

public class AddImagesFilter : IMultiImageFilter<Rgba32>
{
    public string Name => "Add Image...";

    public string Category => "Binary Operations";

    public void Execute(ProcessContext<Rgba32> context)
    {
        var sourceImage = context.SourceImage;
        var secondaryImage = context.SecondaryImage;
        if (secondaryImage == null)
            return;

        int width = Math.Min(sourceImage.Width, secondaryImage.Width);
        int height = Math.Min(sourceImage.Height, secondaryImage.Height);

        Parallel.For(0, height, y =>
        {
            var sourceRow = sourceImage.GetRowSpan(y);
            var secondaryRow = secondaryImage.GetRowSpan(y);


            for (int x = 0; x < width; x++)
            {
                var sourcePixel = sourceRow[x];
                var secondPixel = secondaryRow[x];

                byte newR = (byte)Math.Min(255, sourcePixel.R + secondPixel.R);
                byte newG = (byte)Math.Min(255, sourcePixel.G + secondPixel.G);
                byte newB = (byte)Math.Min(255, sourcePixel.B + secondPixel.B);
                byte newA = (byte)Math.Min(255, sourcePixel.A + secondPixel.A);

                sourceRow[x] = new Rgba32(newR, newG, newB, newA);
            }

        });


    }
}
