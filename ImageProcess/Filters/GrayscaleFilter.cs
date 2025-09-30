using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;


namespace ImageProcess.Filters;

public struct GrayscaleFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "Gri Tonlama";
    public readonly string Category => "Renk Ayarları";

    public readonly void Execute(ProcessContext<Rgba32> context)
    {
        context.SourceImage.ProcessRows(this, context.ROI);
    }

    public readonly void ProcessRow(Span<Rgba32> row)
    {
        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];


            double grayTone = (pixel.R * 0.299) + (pixel.G * 0.587) + (pixel.B * 0.114);

            byte grayByte = (byte)grayTone;


            row[x] = new Rgba32(grayByte, grayByte, grayByte, pixel.A);
        }
    }
}
