using System.ComponentModel;

namespace ImageProcess.Filters.Settings;

public class GaussianBlurSettings
{
    [Category("Parameters")]
    [DisplayName("Radius")]
    [Description("Radius")]
    [DefaultValue(3)]
    public int Radius { get; set; } = 3;
}
