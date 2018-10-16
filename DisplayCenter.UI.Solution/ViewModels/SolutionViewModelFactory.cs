using DisplayCenter.Core;
using DisplayCenter.Core.Internal;
using DisplayCenter.Core.Options;
using DisplayCenter.UI.Core;
using DisplayCenter.UI.Solution.Interactivity;
using DisplayCenter.UI.Solution.Views;
using Microsoft.Extensions.Options;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dc = DisplayCenter.Core;

namespace DisplayCenter.UI.Solution.ViewModels
{
    public interface ISolutionViewModelFactory
    {
        SolutionViewModel Create(dc.Solution sln);
    }

    public class SolutionViewModelFactory : ISolutionViewModelFactory
    {
        private readonly InteractionRequest<PopupCachedWindowNotification> _request;
        private readonly SolutionCardViewOptions _solutionCardViewOptions;
        private readonly CachedImageViewOptions _classifyViewOptions;
        private readonly IImageFactory _imageFactory;
        private readonly ClassifyGroup _defaultClassifyGroup;

        public SolutionViewModelFactory(
            IOptions<SolutionCardViewOptions> solutionCardViewOptions,
            IOptions<CachedImageViewOptions> classifyViewOptions,
            IOptionsSnapshot<ClassifyGroup> defaultClassifyGroupOptions,
            IImageFactory imageFactory,
            InteractionRequest<PopupCachedWindowNotification> request)
        {
            _request = NamedNullException.Assert(request, nameof(request));
            _solutionCardViewOptions = NamedNullException.Assert(solutionCardViewOptions, nameof(solutionCardViewOptions)).Value;
            _classifyViewOptions = NamedNullException.Assert(classifyViewOptions?.Value, nameof(classifyViewOptions));
            _imageFactory = NamedNullException.Assert(imageFactory, nameof(imageFactory));
            _defaultClassifyGroup = NamedNullException.Assert(defaultClassifyGroupOptions.Get("default"), nameof(defaultClassifyGroupOptions));
        }

#warning 下面的Create函数集成了很多个工厂，在修改时要注意
        public SolutionViewModel Create(dc.Solution sln)
        {
            return new SolutionViewModel(
                sln,
                new SolutionClassifyViewModel(
                    sln,
                    _classifyViewOptions.CardWidth, _classifyViewOptions.CardHeight,
                    _imageFactory,
                    new ClassifyGroupLocator(sln.Classes, _defaultClassifyGroup)),
                new PieHelper(sln.Classes.Concat(new[] { _defaultClassifyGroup })),
                _solutionCardViewOptions,
                _request);
        }
    }
}
