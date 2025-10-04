using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;

namespace ImageProcess.Filters.Detail;

public struct SharpenFilter : IFilter<Rgba32>
{
    public readonly string Name => "Sharpen Filter";

    public readonly string Category => "Detail";

    private static float[,] Kernel => new float[,]
    {
        { 0, -1, 0 },
        { -1, 5, -1 },
        { 0, -1, 0 }
    };
    public void Execute(ProcessContext<Rgba32> context)
    {
        context.SourceImage.Convolve(Kernel, context.ROI);
    }

}
