using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace ProjektyElektronika.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            IncrementCommand = new Command(() => N1++);
        }

        private int _n1;
        public int N1
        {
            get => _n1;
            set => SetProperty(ref _n1, value);
        }

        public ICommand IncrementCommand { get; }
    }
}
