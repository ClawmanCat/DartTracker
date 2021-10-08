using DartTracker.Models;
using DartTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DartTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string playerOne;
        private string playerTwo;

        public App currentApp = Application.Current as App;
        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
            setBindings();
        }

        private void TextChangedEventHandler1(object sender, TextChangedEventArgs args)
        {
            playerOne = p1label.Text;
        }
        private void TextChangedEventHandler2(object sender, TextChangedEventArgs args)
        {
            playerTwo = p2label.Text;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        public void setBindings()
        {
            Tournament tournament = currentApp.tournament;
            var currentLeg = tournament.Games[0].gameSets[0].legs[0]; // use Tournament View model to pass trough current legg
            playerOne = tournament.Players[0].Name;
            playerTwo = tournament.Players[1].Name;

            DataContext = new
            {
                leg = new GameLegViewModel(tournament.Players, currentLeg)
                // prefevrably use the tournament also this way; 
            };

            p1label.Text = playerOne;
            p2label.Text = playerTwo;
        }

    }
}
