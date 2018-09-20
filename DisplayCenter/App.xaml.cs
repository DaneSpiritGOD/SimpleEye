using DisplayCenter.Views;
using Microsoft.Extensions.DependencyInjection.Prism;

namespace DisplayCenter
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication<MainWindow>
    {
        public App(
            IHostBuilderAdapter hostBuilder)
            : base(hostBuilder) { }
    }
}
