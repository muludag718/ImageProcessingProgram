
namespace ImageProcess.Core.Interfaces;

/// <summary>
/// Bir görüntü satırını (Span<TPixel>) işleyebilen operasyonlar için bir kontrat tanımlar.
/// Performans için struct'lar tarafından uygulanması hedeflenir.
/// </summary>
public interface IRowProcessor<TPixel> where TPixel : struct, IPixel<TPixel>
{
    void ProcessRow(Span<TPixel> row);
}
