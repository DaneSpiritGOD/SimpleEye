using DisplayCenter.Core;
using DisplayCenter.Core.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DisplayCenter.Core
{
    public interface ILocator<TKey, TTarget>
    {
        TTarget Locate(TKey item);
    }

    public interface ISolutionLocator : ILocator<ImageDto, Solution>, ILocator<int, Solution> { }

    public interface IClassifyGroupLocator : ILocator<int, ClassifyGroup>, ILocator<int?, ClassifyGroup>
    {
        ClassifyGroup DefaultGroup { get; }
    }
}
