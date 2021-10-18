using DartTracker.Models;
using DartTracker.ViewModels;
using System;
using System.Windows.Input;

namespace DartTracker.Commands
{
    class SerializeTournamentCommand : ICommand
    {
        private TournamentViewModel _viewModel;
        public SerializeTournamentCommand(TournamentViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            
            return _viewModel.CanSerializeTournament();
        }

        public void Execute(object parameter)
        {
            _viewModel.Serialize();
        }
    }
}
