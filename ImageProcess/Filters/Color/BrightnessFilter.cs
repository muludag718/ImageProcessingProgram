using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;
using ImageProcess.Filters.Color.Settings;
using ImageProcess.Filters.ColorSettings.Settings;

namespace ImageProcess.Filters.Color;

public struct BrightnessFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "Brightness...";

    public readonly string Category => "Color";
    public BrightnessSettings Settings { get; private set; }

    public BrightnessFilter() => Settings = new();

    public readonly void Execute(ProcessContext<Rgba32> context)
    {
        context.SourceImage.ProcessRows(this, context.ROI);
    }

    public readonly void ProcessRow(Span<Rgba32> row)
    {
        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];

            // Değişken isimlerini daha kısa tutabiliriz.
            var newR = pixel.R * Settings.Amount;
            var newG = pixel.G * Settings.Amount;
            var newB = pixel.B * Settings.Amount;

            byte byteR = (byte)Math.Clamp(newR, 0, 255);
            byte byteG = (byte)Math.Clamp(newG, 0, 255);
            byte byteB = (byte)Math.Clamp(newB, 0, 255);

            row[x] = new Rgba32(byteR, byteG, byteB, pixel.A);
        }
    }
}
