using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayCenter.UI.Solution.ViewModels
{
    public class TabItemViewModel
    {
        public string Header { get; set; }
        public ObservableCollection<DetailedImageModel> Content { get; set; }
    }
}
