using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayCenter.Core.Options
{
    public class SolutionClusterOptions
    {
        public SolutionCardViewOptions SolutionCardView { get; set; }
        public ClassifyViewOptions ClassifyView { get; set; }
        public IEnumerable<SolutionOptions> Solutions { get; set; }

        public SolutionOptions DefaultSolution { get; set; }
        public ClassifyGroup DefaultClassifyGroup { get; set; }
        public IEnumerable<ClassifyGroup> ClassifyGroups { get; set; }
    }

    public class ClassifyGroup
    {
        public int Id { get; set; }
        public string Display { get; set; }
        public string Color { get; set; }
        public int MaxCacheCount { get; set; }
        public bool DisableDisplayInFront { get; set; }
        public string AlterKey { get; set; }
        public string AddToSummaryGroup { get; set; }
    }
}
