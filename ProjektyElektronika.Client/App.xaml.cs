using System;
using System.Globalization;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Client.ViewModels;
using ProjektyElektronika.Client.Views;

namespace ProjektyElektronika.Client
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var ci = new System.Globalization.CultureInfo("pl-PL");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

            //fix language in UI
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            _serviceProvider = AddServices().BuildServiceProvider();
        }

        private IServiceCollection AddServices()
        {
            var services = new ServiceCollection();

            //add service definitions here
            services.AddScoped<IDataProvider, DataProvider>();
            return services;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var viewModel = ActivatorUtilities.GetServiceOrCreateInstance<MainViewModel>(_serviceProvider);
            var view = new MainWindow
            {
                DataContext = viewModel
            };
            view.Show();

            base.OnStartup(e);
        }
    }
}
