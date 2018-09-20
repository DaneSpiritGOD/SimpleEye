using DisplayCenter.Core.Control;
using DisplayCenter.UI.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DisplayCenter.UI.Core
{
    public class CommonCommandCollection : BindableBase, ICommonCommandCollection
    {
        public CommonCommandCollection(
            IHostControl hostControl,
            ISummaryControl summaryControl,
            IImageControl imageControl,
            Application app)
        {
            _hostControl = NamedNullException.Assert(hostControl, nameof(hostControl));
            _summaryControl = NamedNullException.Assert(summaryControl, nameof(summaryControl));
            _imageControl = NamedNullException.Assert(imageControl, nameof(imageControl));
            _dispatcher = app.Dispatcher;

            ClearCachedImageCommand = new DelegateCommand(async () => await _imageControl.ClearCahedImagesAsync());
            ClearSummaryCommand = createCommand(ControlType.ClearSummary);
            RestartAllHostCommand = createCommand(ControlType.RestartAll);
            ShutdownAllHostCommand = createCommand(ControlType.ShutdownAll);
        }

        private readonly IHostControl _hostControl;
        private readonly ISummaryControl _summaryControl;
        private readonly IImageControl _imageControl;
        private readonly Dispatcher _dispatcher;

        public DelegateCommand ClearCachedImageCommand { get; }
        public DelegateCommand ClearSummaryCommand { get; }
        public DelegateCommand RestartAllHostCommand { get; }
        public DelegateCommand ShutdownAllHostCommand { get; }

        private bool _canSendCommand = true;
        public bool CanSendCommand
        {
            get => _canSendCommand;
            private set => SetProperty(ref _canSendCommand, value);
        }

        public void UpdateCanSendCommand(bool value)
        {
            _dispatcher.BeginInvoke(new Action<bool>(updateCanSendCommand), value);

            void updateCanSendCommand(bool innerValue)
            {
                CanSendCommand = innerValue;
            }
        }

        private DelegateCommand createCommand(ControlType type)
        {
            return new DelegateCommand(
                async () => await control(type),
                () => CanSendCommand)
                .ObservesProperty(() => CanSendCommand);
        }

        private async Task control(ControlType type)
        {
            switch (type)
            {
                case ControlType.ClearSummary:
                    await _summaryControl.ClearSummaryAsync();
                    break;
                case ControlType.ShutdownAll:
                    await _hostControl.ShutdownAllAsync();
                    break;
                case ControlType.RestartAll:
                    await _hostControl.RestartAllAsync();
                    break;
                default:
                    throw new NotImplementedException("ukown control type.");
            }
        }

        private enum ControlType
        {
            ClearSummary,
            ShutdownAll,
            RestartAll,
        }
    }
}
