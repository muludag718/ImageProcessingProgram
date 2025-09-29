
using ImageProcess.Core.Models;

namespace ImageProcess.Core.Interfaces;

public interface IOperation<TPixel> where TPixel : struct, IPixel<TPixel>
{
    /// <summary>
    /// İşlemi verilen görüntü üzerinde uygular.
    /// </summary>
    void Execute(ProcessContext<TPixel> context);
}