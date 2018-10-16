using DisplayCenter;
using DisplayCenter.Core;
using DisplayCenter.Core.Options;
using DisplayCenter.UI.Core;
using DisplayCenter.UI.Solution.Interactivity;
using DisplayCenter.UI.Solution.Views;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using dc = DisplayCenter.Core;

namespace DisplayCenter.UI.Solution.ViewModels
{
    public class SolutionViewModel : BindableBase
    {
        public dc.Solution Solution { get; }

        private DetailedImageModel _updatedImageModel;
        public DetailedImageModel UpdatedImageModel
        {
            get => _updatedImageModel;
            private set => SetProperty(ref _updatedImageModel, value);
        }

        private int _updatedFlowVelocity;
        public int UpdatedFlowVelocity
        {
            get => _updatedFlowVelocity;
            private set => SetProperty(ref _updatedFlowVelocity, value);
        }

        public string FlowVelocityUnit { get; }
        public int ImageWidth { get; }
        public ICommand ShowRelatedCachedImageCommand { get; }
        public SolutionClassifyViewModel ClassifiedImageVM { get; }

        public PieHelper PieHelper { get; }

        public bool ImageViewShowStatusBar { get; }
        public bool ShowImageInfoColor { get; }

        private readonly InteractionRequest<PopupCachedWindowNotification> _request;
        public SolutionViewModel(
            dc.Solution solution,
            SolutionClassifyViewModel solutionClassifyViewModel,
            PieHelper pieHelper,
            SolutionCardViewOptions solutionCardViewOptions,
            InteractionRequest<PopupCachedWindowNotification> request)
        {
            _request = NamedNullException.Assert(request, nameof(request));
            Solution = NamedNullException.Assert(solution, nameof(solution));

            ShowRelatedCachedImageCommand = new DelegateCommand(
                () => _request.Raise(new PopupCachedWindowNotification { Title = Solution.Display, Content = ClassifiedImageVM }));
            ClassifiedImageVM = NamedNullException.Assert(solutionClassifyViewModel, nameof(solutionClassifyViewModel));
            PieHelper = NamedNullException.Assert(pieHelper, nameof(pieHelper));

            ImageViewShowStatusBar = solutionCardViewOptions.ShowImageViewStatusBar;
            ShowImageInfoColor = solutionCardViewOptions.ShowImageInfoColor;
            FlowVelocityUnit = solutionCardViewOptions.FlowVelocityUnit;
            ImageWidth = solutionCardViewOptions.ImageWidth;
        }

        public void Add(ImageDto dto)
        {
            var options = ClassifiedImageVM.Add(dto, out var im);
            if (im != default)
            {
                UpdatedImageModel = im;
            }
            if (options.AddToSummaryPie)
            {
                PieHelper.Trigger(options.Display);
            }
        }

        public void ClearImage()
        {
            UpdatedImageModel = null;
            ClassifiedImageVM.ClearImage();
        }

        public void ClearSummary()
        {
            PieHelper.Clear();
        }
    }
}
