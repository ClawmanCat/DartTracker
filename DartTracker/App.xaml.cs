using DartTracker.Models;
using DartTracker.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DartTracker.ViewModels;

namespace DartTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application , INotifyPropertyChanged
    {
        private Tournament _tournament;
        public Tournament tournament { 
            get { return _tournament; } 
            set { _tournament = value; OnPropertyChanged("tournament"); }  
        }
        public Score score { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _createGameObject = false;
        public bool CreateGameObject
        {
            get { return _createGameObject; }
            set { _createGameObject = value; OnPropertyChanged("CreateGameObject"); }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //StartNewTournament();
            //setup score
            score = new Score();

            // Tournament setup
            tournament = new Tournament();
            tournament.GamesToWin = 1;
            tournament.Games = new List<Game>(1);
            tournament.Players = new List<Player>(2) { new Player(), new Player() };
            //tournament.TimeAndDate = DateTime.Now;
            tournament.Winner = null;

            // Initializing the UserInput
            UserInput userInputWindow = new();

            UserInputWindowViewModel userinputViewModel = new UserInputWindowViewModel(tournament, score);
            userInputWindow.DataContext = userinputViewModel;
            // Opening the UserInput Window
            bool? res = userInputWindow.ShowDialog();
            // If the UserInput Window is closed, open the next Window
            if (res == true)
            {
                // Opening the MainWindow
                MainWindow mainWindow = new();
                mainWindow.ShowDialog();
            }
            else
            {
                Shutdown();
            }
        }
        public void StartNewTournament()
        {
            //emptying objects
            score = null;
            tournament = null;
            _createGameObject = false;
            //setup score
            score = new Score();
            // Tournament setup
            tournament = new Tournament();
            tournament.GamesToWin = 1;
            tournament.Games = new List<Game>(1);
            tournament.Players = new List<Player>(2) { new Player(), new Player() };
            //tournament.TimeAndDate = DateTime.Now;
            tournament.Winner = null;

            // Initializing the UserInput
            UserInput userInputWindow = new();

            UserInputWindowViewModel userinputViewModel = new UserInputWindowViewModel(tournament, score);
            userInputWindow.DataContext = userinputViewModel;
            // Opening the UserInput Window
            bool? res = userInputWindow.ShowDialog();
            // If the UserInput Window is closed, open the next Window
            if (res == true)
            {
                // Opening the MainWindow
                MainWindow mainWindow = new();
                mainWindow.ShowDialog();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    }
}
