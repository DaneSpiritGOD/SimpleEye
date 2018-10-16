using DisplayCenter.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayCenter.Core
{
    public class Solution
    {
        public int Id { get; }
        public string Display { get; }
        public int Order { get; }
        public IEnumerable<ClassifyGroup> Classes { get; }

        public static readonly IEqualityComparer<Solution> Comparer = new SolutionComparer();

        internal Solution(SolutionOptions options) : this(options.Id, options.Order, options.Display, options.ClassifyGroups) { }

        public Solution(int id, int order, string display, IEnumerable<ClassifyGroup> classes)
        {
            Id = id;
            Order = order;
            Display = StringNullOrWhiteSpaceException.Assert(display, nameof(Display));
            Classes = classes;
        }

        public override string ToString()
        {
            return Display;
        }

        private class SolutionComparer : IEqualityComparer<Solution>
        {
            public bool Equals(Solution x, Solution y)
            {
                NamedNullException.Assert(x, nameof(x));
                NamedNullException.Assert(y, nameof(y));
                if (x == y)
                {
                    return true;
                }

                StringNullOrWhiteSpaceException.Assert(x.Display, nameof(Display));
                StringNullOrWhiteSpaceException.Assert(y.Display, nameof(Display));

                return x.Id == y.Id &&
                    x.Order == y.Order &&
                    StringComparer.OrdinalIgnoreCase.Equals(x.Display, y.Display);
            }

            public int GetHashCode(Solution obj)
            {
                return obj.Display.ToLower().GetHashCode() * obj.Id;
            }
        }
    }
}
