using Archiving.ConfigureService;
using AutoMapper;
using DisplayCenter.Core;
using DisplayCenter.Core.Control;
using DisplayCenter.Core.Internal;
using DisplayCenter.Core.Options;
using DisplayCenter.Extensions.Scanner;
using DisplayCenter.ImageService;
using DisplayCenter.UI.Core;
using DisplayCenter.UI.Solution;
using DisplayCenter.UI.Solution.Interactivity;
using DisplayCenter.UI.Solution.Internal;
using DisplayCenter.UI.Solution.ViewModels;
using DisplayCenter.UI.Solution.Views;
using DisplayCenter.ViewModels;
using DisplayCenter.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Prism;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NanomsgPlus;
using Newtonsoft.Json;
using NLog;
using NLog.Extensions.Logging;
using Prism.Interactivity.InteractionRequest;
using Prism.Ioc;
using Suites.Core.Process;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace DisplayCenter
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var logger = LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("程序启动");

                var app = CreateApp(args);
                app.InitializeComponent();
                app.Run();
            }
            catch (ProcessNotSingletonException pnse)
            {
                logger.Error(pnse, "process not singleton.");
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Fatal(ex.ToString());
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        public static App CreateApp(string[] args)
        {
            return HostBuilderExt.CreateDefaultHostBuilder(args)
            .ForceProcessSingleton()
            .UseScanner("Scanner")
            .ConfigureServices((hostContext, services) =>
            {
                configureImageService(hostContext, services);

                services.AddHostedMediatR();
                services.AddHostedAutoMapper();

                services.AddObserverOperationFactoryExtension<InprocOperationFactoryExtension>();

                services.Configure<SolutionClusterOptions>(hostContext.Configuration.GetSection("SolutionCluster"));
                services.Configure<ClassifyGroup>("default", hostContext.Configuration.GetSection("SolutionCluster:DefaultClassifyGroup"));
                services.AddSingleton<ISolutionLocator, SolutionLocator>();

                services.Configure<SolutionCardViewOptions>(hostContext.Configuration.GetSection("SolutionCluster:SolutionCardView"));
                services.Configure<CachedImageViewOptions>(hostContext.Configuration.GetSection("SolutionCluster:CachedImageView"));

                services.AddSingleton<IHostControl, DefaultHostControl>();

                services.AddSingleton<ICommonCommandCollection, CommonCommandCollection>();

                services.AddSingleton<IImageFactory, DefaultImageVMFactory>();

                services.Configure<WindowOptions>(hostContext.Configuration.GetSection("Window"));
                services.AddSingleton(new InteractionRequest<PopupCachedWindowNotification>());
                services.AddSingleton<ISolutionViewModelFactory, SolutionViewModelFactory>();

                services.AddSingleton<SolutionViewModelManager>();
                services.AddSingleton<ISolutionViewModelManager>(provider => provider.GetService<SolutionViewModelManager>());
                services.AddSingleton<IImageControl>(provider => provider.GetService<SolutionViewModelManager>());
                services.AddSingleton<ISummaryControl>(provider => provider.GetService<SolutionViewModelManager>());

                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .UseHostedService<HostedImageService>()
            .UseWpf<App>();
        }

        private static void configureImageService(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddImageChannel(ctx.Configuration.GetValue("ImageService:BoundedChannelCapacity", 100));
            services.Configure<ImageServiceOptions>(ctx.Configuration.GetSection("ImageService"));
            services.AddSingleton<IImageDtoTransformer, ImageDtoFactory>();
        }
    }
}
