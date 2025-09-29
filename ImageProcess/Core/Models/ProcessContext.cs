
using ImageProcess.Core.Interfaces;
using ImageProcess.Filters;

namespace ImageProcess.Core.Models;

public class ProcessContext<TPixel> where TPixel : struct, IPixel<TPixel>
{
    public required AdvancedBitmap<TPixel> SourceImage { get; set; }
    public AdvancedBitmap<TPixel>? SecondaryImage { get; set; }
    public Rectangle ROI { get; set; }


}
