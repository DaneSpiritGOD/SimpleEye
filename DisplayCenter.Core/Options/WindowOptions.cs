using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayCenter.Core.Options
{
    public class WindowOptions
    {
        public string Caption { get; set; }
        public int CaptionFontSize { get; set; }
        public bool TopMost { get; set; }
        public bool ExtensionAreaShowOnStartup { get; set; }
    }
}
