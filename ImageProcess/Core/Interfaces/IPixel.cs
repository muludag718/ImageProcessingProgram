
using ImageProcess.Core.Models;

namespace ImageProcess.Core.Interfaces;

public interface IPixel<T> where T : struct, IPixel<T>
{
    void FromRgba32(Rgba32 source);

    Rgba32 ToRgba32();
}
