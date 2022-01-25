using System;
using Microsoft.Extensions.DependencyInjection;
using MvvmHelpers;

namespace ProjektyElektronika.Client.ViewModels
{
    public class Navigation
    {
        private readonly IServiceProvider _serviceProvider;

        public Navigation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public event Action CurrentViewModelChanged;

        public void Navigate<T>() where T : BaseViewModel
        {
            CurrentViewModel = ActivatorUtilities.GetServiceOrCreateInstance<T>(_serviceProvider);
        }
    }
}