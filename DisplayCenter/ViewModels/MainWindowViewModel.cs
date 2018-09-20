using AutoMapper;
using DisplayCenter.Core;
using DisplayCenter.Core.Options;
using DisplayCenter.ImageService;
using DisplayCenter.UI.Core;
using DisplayCenter.UI.Solution.Interactivity;
using DisplayCenter.UI.Solution.ViewModels;
using DisplayCenter.UI.Solution.Views;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prism;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace DisplayCenter.ViewModels
{
    internal partial class MainWindowViewModel : BindableBase
    {
        private string _status;
        public string Status
        {
            get => _status;
            private set => SetProperty(ref _status, value);
        }

        #region Window UI
        public bool TopMost { get; }
        public string Caption { get; }
        public int CaptionFontSize { get; }
        public bool ShowExtensionArea { get; set; }

        public int SVMCardHeight { get; }
        public int SVMCardWidth { get; }
        public int SVMCardFontSize { get; }
        #endregion Window UI

        #region Commands
        private bool _dialogResult;
        public bool DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        public DelegateCommand<System.ComponentModel.CancelEventArgs> BeforeClosingCommand { get; }
        private void mustDoneBeforCloseWindow(System.ComponentModel.CancelEventArgs arg)
        {
            //try
            //{
            //    _logger.Info(Properties.Resources.PrepareClosing);
            //    _securityService.VerifyAdminAuthority();
            //    _logger.Warn(Properties.Resources.HaveClosed);
            //}
            //catch (SecurityException)
            //{
            //    arg.Cancel = true;
            //    _logger.Info(Properties.Resources.FailClose);
            //}
            //Dispose();
        }

        public DelegateCommand QuitCommand { get; }

        public ICommonCommandCollection CommonCommandCollection { get; }
        #endregion Commands

        private readonly ILogger<MainWindowViewModel> _logger;
        private readonly ISolutionViewModelManager _svmMgr;

        public ObservableCollection<SolutionViewModel> SolutionViewModels => _svmMgr.SVMs;
        public InteractionRequest<PopupCachedWindowNotification> PopupCachedImageWindowRequest { get; }

        public MainWindowViewModel(
            IOptions<WindowOptions> windowOptions,
            IOptions<SolutionCardViewOptions> slnCardOptions,
            ICommonCommandCollection commonCommandCollection,
            ISolutionViewModelManager svmMgr,
            InteractionRequest<PopupCachedWindowNotification> request,
            ILogger<MainWindowViewModel> log)
        {
            _logger = NamedNullException.Assert(log, nameof(log));
            var windowOptions_ = NamedNullException.Assert(windowOptions, nameof(windowOptions)).Value;
            var slnOptions = NamedNullException.Assert(slnCardOptions, nameof(slnCardOptions)).Value;
            _svmMgr = NamedNullException.Assert(svmMgr, nameof(svmMgr));

            PopupCachedImageWindowRequest = NamedNullException.Assert(request, nameof(request));

            _status = string.Empty;

            #region Window UI
            TopMost = windowOptions_.TopMost;
            CaptionFontSize = windowOptions_.CaptionFontSize;
            Caption = windowOptions_.Caption;
            ShowExtensionArea = windowOptions_.ExtensionAreaShowOnStartup;

            SVMCardHeight = slnOptions.Height;
            SVMCardWidth = slnOptions.Width;
            SVMCardFontSize = slnOptions.FontSize;
            #endregion Window UI            

            #region Command
            BeforeClosingCommand = new DelegateCommand<System.ComponentModel.CancelEventArgs>(mustDoneBeforCloseWindow);
            DialogResult = false;
            QuitCommand = new DelegateCommand(() => DialogResult = true);

            CommonCommandCollection = NamedNullException.Assert(commonCommandCollection, nameof(commonCommandCollection));
            #endregion Command
        }
    }
}
