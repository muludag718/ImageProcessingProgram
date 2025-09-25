

namespace ImageProcess.Core.Interfaces;

/// <summary>
/// Tüm filtre sınıflarının uyması gereken kontratı tanımlar.
/// IOperation arayüzünden kalıtım alarak tembel değerlendirme zincirine dahil olabilir.
/// </summary>
public interface IFilter<TPixel> : IOperation<TPixel> where TPixel : struct, IPixel<TPixel>
{
    /// <summary>
    /// Filtrenin kullanıcı arayüzünde görünecek adı.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Filtrenin menülerde gruplanacağı kategori.
    /// </summary>
    string Category { get; }
}
