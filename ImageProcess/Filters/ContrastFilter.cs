
using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;

namespace ImageProcess.Filters;

public struct ContrastFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "ContrastFilter";

    public readonly string Category => "Filter";



    //public ContrastFilter(double contrastFactor)
    //{
    //    this.ContrastFactor = contrastFactor;
    //}

    public void Execute(AdvancedBitmap<Rgba32> image)
    {
        image.ProcessRows(this);
    }

    public void ProcessRow(Span<Rgba32> row)
    {
        double ContrastFactor = 25;
        double factor = (255.0 + ContrastFactor) / 255.0;

        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];

            var R = (int)((((pixel.R / 255.0) - 0.5) * factor + 0.5) * 255.0);
            var G = (int)((((pixel.G / 255.0) - 0.5) * factor + 0.5) * 255.0);
            var B = (int)((((pixel.B / 255.0) - 0.5) * factor + 0.5) * 255.0);

            byte byteR = (byte)Math.Min(255, Math.Max(0, R));
            byte byteG = (byte)Math.Min(255, Math.Max(0, G));
            byte byteB = (byte)Math.Min(255, Math.Max(0, B));

            row[x] = new Rgba32(byteR, byteG, byteB, pixel.A);

        }
    }
}
