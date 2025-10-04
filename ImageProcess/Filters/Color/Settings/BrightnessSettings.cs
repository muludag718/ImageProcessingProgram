using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcess.Filters.Color.Settings;

public class BrightnessSettings
{
    [Category("Parameters")]
    [DisplayName("Amount")]
    [Description("Adjusts the image brightness. 1.0 is neutral; higher values increase brightness, lower values decrease it.")]
    [DefaultValue(1.0f)]
    public float Amount { get; set; } = 1.0f;
}
