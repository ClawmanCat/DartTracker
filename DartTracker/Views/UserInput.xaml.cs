using DartTracker.Models;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace DartTracker.Views
{
    /// <summary>
    /// Interaction logic for UserInput.xaml
    /// </summary>
    public partial class UserInput : Window
    {
        Tournament userInputTournament;
        public UserInput(Tournament tournament)
        {
            InitializeComponent();
            userInputTournament = tournament;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// This function is triggered when the user presses the "OK" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                userInputTournament.Players[0] = p1;
                userInputTournament.Players[1] = p2;
                try
                {
                    // variables to be used for setting legs and sets
                    int sets = Convert.ToInt32(amountOfSets.Text);
                    int legs = Convert.ToInt32(amountOfLegs.Text);
                    // Setting Game
                    Game game = new Game();
                    game.Winner = null;
                    // Creating empty Gamesets objects
                    game.gameSets = new GameSet[sets];
                    // Filling the empty Gamesets
                    for(int i = 0; i < sets; i++)
                    {
                        GameSet gs = new GameSet();
                        gs.Winner = null;
                        gs.legs = new GameLeg[legs];
                        for(int j = 0; j < legs; j++)
                        {
                            GameLeg lg = new GameLeg();
                            gs.legs[j] = lg;
                        }
                        game.gameSets[i] = gs;
                    }

                    // Adding the Game to the game array in the global model.
                    userInputTournament.Games[0] = game;
                    // 
                    DialogResult = true;
                    Close();
                }
                catch
                {
                    MessageBox.Show("Zorg ervoor dat je een nummer invult bij sets/legs");
                }
            }
        }

        /// <summary>
        /// This regex will only allow numbers.
        /// </summary>
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        /// <summary>
        /// This function uses a regex to make it possible to only insert numbers inside a Textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlyNumbersTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
