using System;
using System.Collections.Generic;
using System.Text;

namespace DisplayCenter.Core.Options
{
    public class ClassifyGroup
    {
        public int Id { get; set; }
        public string Display { get; set; }
        public string Color { get; set; }
        public int MaxCacheCount { get; set; }
        public bool DisableDisplayInFront { get; set; }
        public string AlterKey { get; set; }
        public bool AddToSummaryPie { get; set; }
    }
}
