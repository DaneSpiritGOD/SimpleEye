using DisplayCenter.Core;
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
        private readonly ClassifyViewOptions _classifyViewOptions;
        private readonly IImageFactory _imageFactory;
        private readonly IClassifyGroupLocator _classifyGroupLocator;

        public SolutionViewModelFactory(
            IOptions<SolutionCardViewOptions> solutionCardViewOptions,
            IOptions<ClassifyViewOptions> classifyViewOptions,
            IImageFactory imageFactory,
            InteractionRequest<PopupCachedWindowNotification> request,
            IClassifyGroupLocator classifyGroupLocator)
        {
            _request = NamedNullException.Assert(request, nameof(request));
            _solutionCardViewOptions = NamedNullException.Assert(solutionCardViewOptions, nameof(solutionCardViewOptions)).Value;
            _classifyViewOptions = NamedNullException.Assert(classifyViewOptions?.Value, nameof(classifyViewOptions));
            _imageFactory = NamedNullException.Assert(imageFactory, nameof(imageFactory));
            _classifyGroupLocator = NamedNullException.Assert(classifyGroupLocator, nameof(classifyGroupLocator));
        }

        public SolutionViewModel Create(dc.Solution sln)
        {
            return new SolutionViewModel(
                sln,
                new SolutionClassifyViewModel(
                    sln,
                    _classifyViewOptions.CardWidth, _classifyViewOptions.CardHeight,
                    _imageFactory,
                    _classifyGroupLocator),
                new SolutionSummaryViewModel(),
                _solutionCardViewOptions,
                _request);
        }
    }
}
