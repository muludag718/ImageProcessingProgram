using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;


namespace ImageProcess.Filters;

public struct InvertFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "Renkleri Ters Çevir";
    public readonly string Category => "Renk Ayarları";



    public readonly void Execute(ProcessContext<Rgba32> context)
    {
        context.SourceImage.ProcessRows(this, context.ROI);
    }

    public readonly void ProcessRow(Span<Rgba32> row)
    {
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
