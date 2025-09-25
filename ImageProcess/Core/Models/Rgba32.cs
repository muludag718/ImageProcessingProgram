
using ImageProcess.Core.Interfaces;
using System.Runtime.InteropServices;

namespace ImageProcess.Core.Models;

[StructLayout(LayoutKind.Sequential)]
public struct Rgba32 : IPixel<Rgba32>
{
    public byte B, G, R, A;

    public Rgba32(byte r, byte g, byte b, byte a)
    {
        // Parametreleri, bellekteki doğru alanlara atar.
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public void FromRgba32(Rgba32 source)
    {
        this = source;
    }

    public Rgba32 ToRgba32() => this;

}
