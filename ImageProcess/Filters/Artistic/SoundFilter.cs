using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;


namespace ImageProcess.Filters.Artistic;

public struct SoundFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "Add Noise...";

    public readonly string Category => "Artistic";

    public readonly void Execute(ProcessContext<Rgba32> context)
    {
        context.SourceImage.ProcessRows(this, context.ROI);
    }

    public readonly void ProcessRow(Span<Rgba32> row)
    {
        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];
            int noiseAmount = Random.Shared.Next(-30, 31); // -30  +30
            byte R = (byte)Math.Min(255, Math.Max(0, pixel.R + noiseAmount));
            byte G = (byte)Math.Min(255, Math.Max(0, pixel.G + noiseAmount));
            byte B = (byte)Math.Min(255, Math.Max(0, pixel.B + noiseAmount));

            row[x] = new Rgba32(R, G, B, pixel.A);
        }
    }
}
