using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;
using ImageProcess.Filters.ColorSettings.Settings;

namespace ImageProcess.Filters.ColorSettings;

public struct ContrastFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "ContrastFilter";

    public readonly string Category => "Filter";

    public ContrastSetting Settings { get; private set; }

    public ContrastFilter() => Settings = new();


    public void Execute(ProcessContext<Rgba32> context)
    {
        context.SourceImage.ProcessRows(this, context.ROI);
    }
    public readonly void ProcessRow(Span<Rgba32> row)
    {
        double factor = (255.0 + Settings.ContrastFactory) / 255.0;

        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];

            var R = (int)(((pixel.R / 255.0 - 0.5) * factor + 0.5) * 255.0);
            var G = (int)(((pixel.G / 255.0 - 0.5) * factor + 0.5) * 255.0);
            var B = (int)(((pixel.B / 255.0 - 0.5) * factor + 0.5) * 255.0);

            byte byteR = (byte)Math.Min(255, Math.Max(0, R));
            byte byteG = (byte)Math.Min(255, Math.Max(0, G));
            byte byteB = (byte)Math.Min(255, Math.Max(0, B));

            row[x] = new Rgba32(byteR, byteG, byteB, pixel.A);

        }
    }


}
