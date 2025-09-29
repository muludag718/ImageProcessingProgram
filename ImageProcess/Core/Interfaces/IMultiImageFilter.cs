using ImageProcess.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcess.Core.Interfaces;

public interface IMultiImageFilter<TPixel> : IFilter<TPixel> where TPixel : struct, IPixel<TPixel>
{
}

