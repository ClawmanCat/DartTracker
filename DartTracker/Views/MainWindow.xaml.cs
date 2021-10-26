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
using DartTracker.Views;

namespace DartTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public App currentApp = Application.Current as App;
        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
            setBindings();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        public void setBindings()
        {
            Tournament tournament = currentApp.tournament;
            Score score = currentApp.score;
            string playerOne = tournament.Players[0].Name;
            string playerTwo = tournament.Players[1].Name;



            DataContext = new
            {
                leg = new MainWindowViewModel(tournament.Players, tournament.Games.Last(), score),
                tournament = new TournamentViewModel(tournament),
                // preferably use the tournament also this way; 
            };

            p1label.Text = playerOne;
            p2label.Text = playerTwo;

        }
        private void p1_LostFocus(object sender, RoutedEventArgs args)
        {
            Tournament tournament = currentApp.tournament;
            TextBox textbox = ((TextBox)sender);
            if (textbox.Text == tournament.Players[0].Name)
            {
                return;
            }
            else if (string.IsNullOrWhiteSpace(textbox.Text))
            {
                MessageBox.Show("The name can not be empty");
                textbox.Text = tournament.Players[0].Name;
            }
            else if (textbox.Text == tournament.Players[1].Name)
            {
                MessageBox.Show("This player is already in the game");
                textbox.Text = tournament.Players[0].Name;
            }
            //else if: Naam komt voor in de geschiedenis. Prompt vraag of dit dezelfde speler is. Ja: verander ID, Nee: verander terug naar tournament.Players[0].Name
            else
            {
                tournament.Players[0].Name = textbox.Text;
            }
        }
        private void p2_LostFocus(object sender, RoutedEventArgs args)
        {
            Tournament tournament = currentApp.tournament;
            TextBox textbox = ((TextBox)sender);

            if (textbox.Text == tournament.Players[1].Name)
            {
                return;
            }
            else if (string.IsNullOrWhiteSpace(textbox.Text))
            {
                MessageBox.Show("The name can not be empty");
                textbox.Text = tournament.Players[1].Name;
            }
            else if (textbox.Text == tournament.Players[0].Name)
            {
                MessageBox.Show("This player is already in the game");
                textbox.Text = tournament.Players[1].Name;
            }
            //else if: Naam komt voor in de geschiedenis. Prompt vraag of dit dezelfde speler is. Ja: verander ID, Nee: verander terug naar tournament.Players[0].Name
            else
            {
                tournament.Players[1].Name = textbox.Text;
            }

        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            StatsWindow statsWindow = new StatsWindow(currentApp.tournament);
            statsWindow.Show();
        }
    }
}
