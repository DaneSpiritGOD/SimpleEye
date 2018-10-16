using DisplayCenter.Core;
using DisplayCenter.Core.Control;
using DisplayCenter.UI.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using dc = DisplayCenter.Core;

namespace DisplayCenter.UI.Solution.ViewModels
{
    public interface ISolutionViewModelManager
    {
        ObservableCollection<SolutionViewModel> SVMs { get; }
        void AddImage(ImageDto dto);
    }

    public class SolutionViewModelManager : ISolutionViewModelManager, IImageControl, ISummaryControl
    {
        private readonly ILogger<SolutionViewModelManager> _logger;
        private readonly Dispatcher _dispatcher;

        public ObservableCollection<SolutionViewModel> SVMs { get; }
        private readonly ISolutionLocator _solutionLocator;
        private readonly ISolutionViewModelFactory _svmFactory;

        public SolutionViewModelManager(
            ISolutionLocator solutionLocator,
            ISolutionViewModelFactory svmFactory,
            Dispatcher dispatcher,
            ILogger<SolutionViewModelManager> logger)
        {
            _svmFactory = NamedNullException.Assert(svmFactory, nameof(svmFactory));
            _solutionLocator = NamedNullException.Assert(solutionLocator, nameof(solutionLocator));
            _logger = NamedNullException.Assert(logger, nameof(logger));

            SVMs = new ObservableCollection<SolutionViewModel>();
            _dispatcher = NamedNullException.Assert(dispatcher, nameof(dispatcher));
        }

        public void AddImage(ImageDto dto)
        {
            NamedNullException.Assert(dto, nameof(dto));

            _dispatcher.BeginInvoke(new Action<ImageDto>(addImageCore), dto);

            void addImageCore(ImageDto innerDto)
            {
                try
                {
                    var svm = resolveSVM(_solutionLocator.Locate(innerDto));
                    svm.Add(innerDto);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }
        }

        private SolutionViewModel resolveSVM(dc.Solution sln)
        {
            var svm = SVMs.SingleOrDefault(x => dc.Solution.Comparer.Equals(x.Solution, sln));
            if (svm == default)
            {
                svm = _svmFactory.Create(sln);
                SVMs.Insert(getNewSVMInsertIndex(sln), svm);
            }
            return svm;
        }

        private int getNewSVMInsertIndex(dc.Solution sln)
        {
            for (int index = 0; index < SVMs.Count; ++index)
            {
                if (sln.Order < SVMs[index].Solution.Order)
                {
                    return index;
                }
            }
            return SVMs.Count;
        }

        public Task ClearCahedImagesAsync()
        {
            foreach (var svm in SVMs)
            {
                svm.ClearImage();
            }
            return Task.CompletedTask;
        }

        public Task ClearSummaryAsync()
        {
            foreach (var svm in SVMs)
            {
                svm.ClearSummary();
            }
            return Task.CompletedTask;
        }
    }
}
