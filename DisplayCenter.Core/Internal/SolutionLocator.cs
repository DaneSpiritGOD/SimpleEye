using DisplayCenter.Core;
using DisplayCenter.Core.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisplayCenter.Core.Internal
{
    public class SolutionLocator : ISolutionLocator
    {
        private readonly IEnumerable<SolutionOptions> _solutionOptionss;
        private readonly ConcurrentDictionary<SolutionOptions, Solution> _solutionTable;
        private readonly SolutionOptions _defaultSolutionOptions;
        private readonly IEnumerable<ClassifyGroup> _classifyGroups;

        public SolutionLocator(IOptions<SolutionClusterOptions> solutionClusterOptions)
        {
            var clusterOptions = NamedNullException.Assert(solutionClusterOptions?.Value, nameof(solutionClusterOptions));

            _solutionTable = new ConcurrentDictionary<SolutionOptions, Solution>(SolutionOptions.Comparer);
            _defaultSolutionOptions = clusterOptions.DefaultSolution;
            _solutionOptionss = clusterOptions.Solutions;
            _classifyGroups = clusterOptions.ClassifyGroups;
        }

        public Solution Locate(ImageDto item)
        {
            NamedNullException.Assert(item, nameof(ImageDto));

            try
            {
                switch (item)
                {
                    case FileImageDto fdto:
                        StringNullOrWhiteSpaceException.Assert(fdto.Key, nameof(fdto.Key));
                        return locate(_solutionOptionss.SingleOrDefault(x => fdto.Key.StartsWith(x.Key, StringComparison.OrdinalIgnoreCase)));
                    case MemoryImageDto mdto:
                        return locate(_solutionOptionss.SingleOrDefault(x => x.Id == mdto.SolutionId));
                    default:
                        throw new NotSupportedException("不支持的数据格式！");
                }
            }
            catch (InvalidOperationException ioex)
            {
                throw new DoubleLocatedLocatorException(ioex.Message);
            }
        }

        public Solution Locate(int id)
        {
            try
            {
                var options = _solutionOptionss.SingleOrDefault(x => x.Id == id);
                return locate(options);
            }
            catch (InvalidOperationException ioex)
            {
                throw new DoubleLocatedLocatorException(ioex.Message);
            }
        }

        private Solution locate(SolutionOptions options)
        {
            if (options == default)
            {
                options = _defaultSolutionOptions;
            }

            if (!_solutionTable.ContainsKey(options))
            {
                _solutionTable[options] = new Solution(options, extract(options));
            }

            return _solutionTable[options];

            IEnumerable<ClassifyGroup> extract(SolutionOptions innerOptions)
            {
                var result = _classifyGroups.Join(innerOptions.ClassifyGroups, x => x.Id, y => y, (x, y) => x).ToList();
                return (result.Count == 0) ? null : result;
            }
        }
    }
}
