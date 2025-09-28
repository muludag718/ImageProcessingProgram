

using System.ComponentModel;

namespace ImageProcess.Filters.Settings;

/// <summary>
/// Constras Factor Value
/// </summary>
public class ContrastSetting
{
    [Category("Parameters")]
    [DisplayName("Contrast Amount")]
    [Description("Contrast level. Negative values ​​reduce contrast, positive values ​​increase it. A value between -100 and 100 is generally used.")]
    [DefaultValue(25)]
    public float ContrastFactory { get; set; } = 25;
}
