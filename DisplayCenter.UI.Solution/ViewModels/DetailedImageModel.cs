using DisplayCenter.Core;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using sw = System.Windows;

namespace DisplayCenter.UI.Solution.ViewModels
{
    public class DetailedImageModel : BindableBase
    {
        public BitmapSource Image { get; }

        public string Time { get; }
        public string Description { get; }
        public string Decorator { get; }
        public string DecoratorDescription { get; }

        public DetailedImageModel(BitmapSource img, string time, string description, string decorator, string type)
        {
            Image = NamedNullException.Assert(img, nameof(img));
            Time = StringNullOrWhiteSpaceException.Assert(time, nameof(time));
            Description = StringNullOrWhiteSpaceException.Assert(description, nameof(description));
            Decorator = StringNullOrWhiteSpaceException.Assert(decorator, nameof(decorator));
            DecoratorDescription = StringNullOrWhiteSpaceException.Assert(type, nameof(type));
        }
    }
}
