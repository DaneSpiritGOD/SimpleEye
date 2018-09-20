using System;
using System.Collections.Generic;
using System.Text;

namespace DisplayCenter.Core.Options
{
    public class SolutionCardViewOptions
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int FontSize { get; set; }

        public bool ShowImageViewStatusBar { get; set; }
        public bool ShowImageInfoColor { get; set; }
        public bool ShowRoiDescription { get; set; }

        public string FlowVelocityUnit { get; set; }
    }
}
