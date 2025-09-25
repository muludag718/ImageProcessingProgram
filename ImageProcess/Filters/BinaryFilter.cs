using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;

namespace ImageProcess.Filters;

public struct BinaryFilter : IFilter<Rgba32>, IRowProcessor<Rgba32>
{
    public readonly string Name => "Binary";
    public readonly string Category => "Renk Ayarları";

    private readonly int Spin;

    public BinaryFilter(int spin)
    {
        this.Spin = spin;
    }

    public void Execute(AdvancedBitmap<Rgba32> image)
    {
        image.ProcessRows(this);
    }

    public void ProcessRow(Span<Rgba32> row)
    {
        for (int x = 0; x < row.Length; x++)
        {
            var pixel = row[x];

        }
    }
}
