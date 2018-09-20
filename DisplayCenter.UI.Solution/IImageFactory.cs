using DisplayCenter.Core;
using DisplayCenter.Core.Options;
using DisplayCenter.UI.Solution.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayCenter.UI.Solution
{
    public interface IImageFactory
    {
        DetailedImageModel ExtractFrom(ImageDto dto, ClassifyGroup group);
    }
}
