using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;

namespace ProjektyElektronika.Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly Navigation _navigation;

        public LoginViewModel(Navigation navigation)
        {
            _navigation = navigation;
            LoginCommand = new AsyncCommand<PasswordBox>(Login);
            GoBackCommand = new Command(GoBack);
        }

        private void GoBack()
        {
            _navigation.Navigate<HomeViewModel>();
        }

        private async Task Login(PasswordBox passwordBox)
        {
            IsBusy = true;
            WebHelpers.Password = passwordBox.Password;
            var isCorrect = await WebHelpers.CheckAdmin();
            if (isCorrect)
            {
                _navigation.Navigate<AdminViewModel>();
            }
            else
            {
                MessageBox.Show("Nie udało się zalogować.");
            }

            IsBusy = false;
        }

        public ICommand LoginCommand { get; }
        public ICommand GoBackCommand { get; }
    }
}
