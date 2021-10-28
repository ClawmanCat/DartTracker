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

        private MainWindow _mainWindow;
        private UserInput _userInputWindow;


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
            StartNewTournament();
        }

        private void ExitMessageBox()
        {
            string message = $"gefleciteerd {tournament.Games.First().Winner.Name} jij hebt gewonnen ! , wil je een nieuwe game starten";
            string caption = "het spel is voorbij";
            MessageBoxButton button = MessageBoxButton.YesNo;

            var result = MessageBox.Show(message, caption, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Environment.Exit(0);
                    StartNewTournament();
                    break;
                case MessageBoxResult.No:
                    Shutdown();
                    break;
            }
        }

        private void StartNewTournament()
        {
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
            _userInputWindow ??= new UserInput();

            UserInputWindowViewModel userinputViewModel = new UserInputWindowViewModel(tournament, score);
            _userInputWindow.DataContext = userinputViewModel;
            // Opening the UserInput Window
            bool? res = _userInputWindow.ShowDialog();
            // If the UserInput Window is closed, open the next Window
            if (res == true)
            {
                // Opening the MainWindow
                _mainWindow ??= new MainWindow();
                bool? resultMainWindow = _mainWindow.ShowDialog();
                if (resultMainWindow == true)
                {
                    ExitMessageBox();
                }

            }
            else
            {
                Shutdown();
            }
        }
        private void CloseAllWindows()
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Close();
        }
    }
}
