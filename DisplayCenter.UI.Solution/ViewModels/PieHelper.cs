using DisplayCenter.Core.Options;
using Prism.Mvvm;
using Suites.Wpf.App.Controls.Pie;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayCenter.UI.Solution.ViewModels
{
    public class PieHelper : BindableBase
    {
        public ObservableCollection<SimpleSeries> SimpleSeries { get; }

        public PieHelper(IEnumerable<ClassifyGroup> classifyGroups)
        {
            NotTrueException.Assert(classifyGroups != default &&
                classifyGroups.Any() &&
                classifyGroups.Select(x => x.Display).Distinct(StringComparer.OrdinalIgnoreCase).Count() == classifyGroups.Count(), nameof(classifyGroups));

            SimpleSeries = new ObservableCollection<SimpleSeries>();
            foreach (var group in classifyGroups)
            {
                if (!group.AddToSummaryPie)
                {
                    continue;
                }
                SimpleSeries.Add(
                    new SimpleSeries
                    {
                        Title = group.Display,
                        Value = 0,
                        Color = group.Color
                    });
            }
        }

        public void Trigger(string title, int count = 1)
        {
            var series = SimpleSeries.SingleOrDefault(x => StringComparer.OrdinalIgnoreCase.Equals(title, x.Title));
            if (series != default)
            {
                series.Value += count;
            }
        }

        public void Clear()
        {
            foreach (var series in SimpleSeries)
            {
                series.Value.Value = 0;
            }
        }
    }
}
