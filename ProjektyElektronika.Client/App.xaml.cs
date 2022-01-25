using System;
using System.Globalization;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Client.ViewModels;
using ProjektyElektronika.Client.Views;

namespace ProjektyElektronika.Client
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            var ci = new System.Globalization.CultureInfo("pl-PL");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

            //fix language in UI
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            _host = new HostBuilder()
                .ConfigureServices(AddServices)
                .Build();
        }

        private void AddServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            services.AddScoped<IDataProvider, DataProvider>();

            var detector = new OnlineDetector();
            services.AddSingleton(detector);
            services.AddHostedService<OnlineDetector>(isp => detector);
            services.AddSingleton<Navigation>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.StartAsync();

            var navigation = _host.Services.GetRequiredService<Navigation>();
            navigation.Navigate<HomeViewModel>();
            var viewModel = ActivatorUtilities.GetServiceOrCreateInstance<MainViewModel>(_host.Services);
            var view = new MainWindow
            {
                DataContext = viewModel
            };
            view.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
        }
    }
}
