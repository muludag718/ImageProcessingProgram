using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;

namespace ImageProcess.Filters.ColorSettings;

public struct SepiaFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "Sepia";

    public readonly string Category => "Color";

    public readonly void Execute(ProcessContext<Rgba32> context)
    {
        context.SourceImage.ProcessRows(this, context.ROI);
    }

    public readonly void ProcessRow(Span<Rgba32> row)
    {
        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];

            var new_R = pixel.R * 0.393 + pixel.G * 0.769 + pixel.B * 0.189;
            var new_G = pixel.R * 0.359 + pixel.G * 0.686 + pixel.B * 0.168;
            var new_B = pixel.R * 0.272 + pixel.G * 0.534 + pixel.B * 0.131;

            byte byte_R = (byte)Math.Clamp(new_R, 0, 255);
            byte byte_G = (byte)Math.Clamp(new_G, 0, 255);
            byte byte_B = (byte)Math.Clamp(new_B, 0, 255);

            row[x] = new Rgba32(byte_R, byte_G, byte_B, pixel.A);
        }
    }
}
