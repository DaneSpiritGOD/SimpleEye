using DisplayCenter.Core.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisplayCenter.Core.Internal
{
    public class ClassifyGroupLocator : IClassifyGroupLocator
    {
        private readonly IReadOnlyDictionary<int, ClassifyGroup> _classifyGroupDict;
        public ClassifyGroup DefaultGroup { get; }

        public ClassifyGroupLocator(
            IOptions<SolutionClusterOptions> solutionClusterOptions)
        {
            var options = NamedNullException.Assert(solutionClusterOptions?.Value, nameof(solutionClusterOptions));
            var groups = options.ClassifyGroups;
            DefaultGroup = NamedNullException.Assert(options.DefaultClassifyGroup, nameof(options.DefaultClassifyGroup));

            var dict = groups.ToDictionary(x => x.Id);
            if (dict.ContainsKey(DefaultGroup.Id))
            {
                throw new RuntimeException($"分类组中不能包含默认分类组的id.");
            }
            else
            {
                dict[DefaultGroup.Id] = DefaultGroup;
            }

            _classifyGroupDict = dict;
        }

        public ClassifyGroup Locate(int? index)
        {
            var index_ = index.HasValue ? index.Value : DefaultGroup.Id;
            return _classifyGroupDict[index_];
        }

        public ClassifyGroup Locate(int index)
        {
            if (!_classifyGroupDict.ContainsKey(index))
            {
                return _classifyGroupDict[DefaultGroup.Id];
            }

            return _classifyGroupDict[index];
        }
    }
}
