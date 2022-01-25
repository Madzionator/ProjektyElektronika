using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Client.Models;
using ProjektyElektronika.Client.Views;

namespace ProjektyElektronika.Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly Navigation _navigation;

        public LoginViewModel(Navigation navigation)
        {
            _navigation = navigation;
            LoginCommand = new Command(Login);
        }

        private void Login()
        {
            //git
            _navigation.Navigate<AdminViewModel>();
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }
    }
}
