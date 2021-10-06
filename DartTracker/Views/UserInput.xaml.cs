using DartTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DartTracker.Views
{
    /// <summary>
    /// Interaction logic for UserInput.xaml
    /// </summary>
    public partial class UserInput : Window
    {
        public App currentApp = Application.Current as App;
        public UserInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This function uses a regex to make it possible to only insert numbers inside a Textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlyNumbersTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(player1.Text) && string.IsNullOrEmpty(player2.Text))
            {
                MessageBox.Show("Both players are empty.");
            }
            else if (string.IsNullOrEmpty(player1.Text))
            {
                MessageBox.Show("Player 1 is empty.");
            }
            else if (string.IsNullOrEmpty(player2.Text))
            {
                MessageBox.Show("Player 2 is empty.");
            }
            else
            {
                Player p1 = new Player();
                Player p2 = new Player();
                p1.Name = player1.Text;
                p2.Name = player2.Text;
                currentApp.tournament.Players.Add(p1);
                currentApp.tournament.Players.Add(p2);
                try
                {
                    // variables to be used for setting legs and sets
                    int setsAmount = Convert.ToInt32(amountOfSets.Text);
                    int legsAmount = Convert.ToInt32(amountOfLegs.Text);
                    // Setting Game
                    Game game = new Game();
                    game.Winner = null;
                    // Creating empty Gamesets objects
                    game.gameSets = new List<GameSet>(setsAmount);
                    for (int i = 0; i < setsAmount; i++)
                    {
                        GameSet gameSet = new GameSet();
                        gameSet.Winner = null;
                        gameSet.legs = new List<GameLeg>(legsAmount);
                        for(int j = 0; j < legsAmount; j++)
                        {
                            GameLeg leg = new GameLeg();
                            leg.history = null;
                            leg.Scores = null;
                            leg.Winner = null;
                            gameSet.legs.Add(leg);
                        }
                        game.gameSets.Add(gameSet);
                    }

                    // Adding the Game to the game array in the global model.
                    currentApp.tournament.Games.Add(game);
                    // 
                    DialogResult = true;
                    Close();
                }
                catch(Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
