using DisplayCenter.Core;
using DisplayCenter.ImageService;
using DisplayCenter.UI.Solution.ViewModels;
using MediatR;
using Microsoft.Extensions.Options;
using Suites.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DisplayCenter.UI.Solution
{
    public class ImageDtoNotificationHandler : INotificationHandler<SimpleNotification<ImageDto>>
    {
        private readonly ISolutionViewModelManager _solutionViewModelManager;

        public ImageDtoNotificationHandler(
            ISolutionViewModelManager solutionViewModelManager)
        {
            _solutionViewModelManager = NamedNullException.Assert(solutionViewModelManager, nameof(solutionViewModelManager));
        }

        public Task Handle(SimpleNotification<ImageDto> notification, CancellationToken cancellationToken)
        {
            _solutionViewModelManager.AddImage(notification.Item);
            return Task.CompletedTask;
        }
    }
}
