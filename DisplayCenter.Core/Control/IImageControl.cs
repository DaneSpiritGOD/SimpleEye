using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DisplayCenter.Core.Control
{
    public interface IImageControl
    {
        Task ClearCahedImagesAsync();
    }
}
