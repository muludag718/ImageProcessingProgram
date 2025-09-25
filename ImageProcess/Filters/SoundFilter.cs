using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace ImageProcess.Filters;

public struct SoundFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "Sound(Gürültü)";

    public readonly string Category => "Filter";

    public readonly void Execute(AdvancedBitmap<Rgba32> image)
    {
        image.ProcessRows(this);
    }

    public void ProcessRow(Span<Rgba32> row)
    {
        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];
            int noiseAmount = Random.Shared.Next(-30, 31); // -30 ile +30 arasında rastgele bir değer
            byte R = (byte)Math.Min(255, Math.Max(0, pixel.R + noiseAmount));
            byte G = (byte)Math.Min(255, Math.Max(0, pixel.G + noiseAmount));
            byte B = (byte)Math.Min(255, Math.Max(0, pixel.B + noiseAmount));

            row[x] = new Rgba32(R, G, B, pixel.A);
        }
    }
}
