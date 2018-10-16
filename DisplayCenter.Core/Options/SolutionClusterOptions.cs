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
        public CachedImageViewOptions CachedImageView { get; set; }
        public IEnumerable<SolutionOptions> Solutions { get; set; }

        public SolutionOptions DefaultSolution { get; set; }
        public ClassifyGroup DefaultClassifyGroup { get; set; }
    }
}
