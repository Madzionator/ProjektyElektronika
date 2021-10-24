using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
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
            _serviceProvider = AddServices().BuildServiceProvider();
        }

        private IServiceCollection AddServices()
        {
            var services = new ServiceCollection();

            //add service definitions here

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
