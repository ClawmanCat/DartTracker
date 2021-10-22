using DartTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DartTracker.ViewModels
{
    public class UserInputWindowViewModel
    {
        // Calling the current app to access the tournament object globally
        public App currentApp = Application.Current as App;
        #region The players object
        private List<Player> _players;
        public List<Player> Players
        {
            get { return _players; }
            set { _players = value; }
        }
        #endregion
        #region The DateTime object
        private DateTime _dateTime;
        public DateTime TournamentDateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; OnPropertyChanged("TournamentDateTime"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public UserInputWindowViewModel()
        {
            Tournament tournament = currentApp.tournament;
            Players = tournament.Players;
            TournamentDateTime = new DateTime(2021, 11, 22);
            tournament.TimeAndDate = TournamentDateTime;
            //TournamentDateTime = tournament.TimeAndDate;
        }
        #region ICommand Members
        private ICommand mUpdater;
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }

        private class Updater : ICommand
        {
            public bool CanExecute(object parameter) => true;
            public event EventHandler CanExecuteChanged;
            public void Execute(object parameter)
            {

            }
        }
        #endregion
    }
}
