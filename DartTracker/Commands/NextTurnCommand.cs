using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DartTracker.ViewModels
{
    class NextTurnCommand : ICommand
    {
        private readonly Action<object> command;
        private readonly Predicate<object> canExecute;
        public NextTurnCommand(Action<object> command) : this(command, null)
        {
        }

        public NextTurnCommand(Action<object> command, Predicate<object> canExecute)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
            this.canExecute = canExecute;
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            command.Invoke(parameter);
        }
    }
}
