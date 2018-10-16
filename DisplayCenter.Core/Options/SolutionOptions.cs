using System;
using System.Collections.Generic;
using System.Text;

namespace DisplayCenter.Core.Options
{
    public class SolutionOptions
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Display { get; set; }
        public int Order { get; set; }
        public ClassifyGroup[] ClassifyGroups { get; set; }

        public static readonly IEqualityComparer<SolutionOptions> Comparer = new SolutionOptionsComparer();

        private class SolutionOptionsComparer : IEqualityComparer<SolutionOptions>
        {
            public bool Equals(SolutionOptions x, SolutionOptions y)
            {
                NamedNullException.Assert(x, nameof(x));
                NamedNullException.Assert(y, nameof(y));
                if (x == y)
                {
                    return true;
                }

                StringNullOrWhiteSpaceException.Assert(x.Key, nameof(Key));
                StringNullOrWhiteSpaceException.Assert(y.Key, nameof(Key));

                return StringComparer.OrdinalIgnoreCase.Equals(x.Key, y.Key) && x.Id == y.Id && x.Order == y.Order;
            }

            public int GetHashCode(SolutionOptions obj)
            {
                return obj.Key.ToLower().GetHashCode() * obj.Id * obj.Order;
            }
        }
    }
}
