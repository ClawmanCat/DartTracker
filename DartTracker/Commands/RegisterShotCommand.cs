using DartTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DartTracker.Commands
{
    class RegisterShotCommand : ICommand
    {
        private GameLegViewModel _viewModel;

        public RegisterShotCommand(GameLegViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            // implement the conditions when a shot counts ect.
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.RegisterShot();
        }
    }
}
