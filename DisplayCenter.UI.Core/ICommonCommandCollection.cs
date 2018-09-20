using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayCenter.UI.Core
{
    public interface ICommonCommandCollection
    {
        DelegateCommand ClearSummaryCommand { get; }
        DelegateCommand ClearCachedImageCommand { get; }
        DelegateCommand RestartAllHostCommand { get; }
        DelegateCommand ShutdownAllHostCommand { get; }

        void UpdateCanSendCommand(bool value);
    }
}
