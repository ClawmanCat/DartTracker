using DartTracker.Utility;
using DartTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DartTracker.Views;

namespace DartTracker.Commands
{
    class RegisterShotCommand : ICommand
    {
        private MainWindowViewModel _viewModel;

        public RegisterShotCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;

            // The input may become valid when it is changed, so listen for changes.
            viewModel.PropertyChanged += (object sender, PropertyChangedEventArgs args) => {
                if (args.PropertyName == "first" || args.PropertyName == "second" || args.PropertyName == "third")
                {
                    EventHandler handler = CanExecuteChanged;
                    if (handler != null) handler(this, new EventArgs());
                }
            };
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return (
                SegmentParser.is_valid_segment(_viewModel.first)  &&
                SegmentParser.is_valid_segment(_viewModel.second) &&
                SegmentParser.is_valid_segment(_viewModel.third)
            );
        }


        public void Execute(object parameter)
        {
            _viewModel.RegisterShot();
            var player = _viewModel.gameLeg.CurrentTurn;
            if (_viewModel.CheckGameWinner(player, player.setsWon))
            {
                var window =(MainWindow)parameter;
                window.DialogResult = true;
                window.Close();

            }
        }
    }
}
