using DartTracker.Models;
using DartTracker.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DartTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Tournament tournament { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Tournament setup
            tournament = new Tournament();
            tournament.GamesToWin = 1;
            tournament.Games = new List<Game>(1);
            tournament.Players = new List<Player>(2);
            tournament.Winner = null;

            // Initializing the UserInput
            UserInput userInput = new UserInput();
            // Opening the UserInput Window
            bool? res = userInput.ShowDialog();
            // If the UserInput Window is closed, open the next Window
            if (res == true)
            {
                // Opening the MainWindow
                MainWindow main = new MainWindow();
                main.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}
