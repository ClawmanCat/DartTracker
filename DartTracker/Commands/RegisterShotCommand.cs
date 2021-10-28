using DartTracker.Utility;
using DartTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        }
    }
    class UndoShotCommand : ICommand
    {
        private MainWindowViewModel _viewModel;

        public UndoShotCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, new EventArgs()); 
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //return true;
            return _viewModel.CheckHistorySize();              
        }

        public void Execute(object parameter)
        {
            _viewModel.UndoShot();
        }
    }
}
