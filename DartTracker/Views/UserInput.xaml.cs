using DartTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
                
                try
                {
                    // Setting the players in the Tournament object
                    currentApp.tournament.Players.Add(new Player() { Name = player1.Text });
                    currentApp.tournament.Players.Add(new Player() { Name = player2.Text });
                    // full DateTime
                    DateTime datetime = new DateTime();
                    // This datetime only fills the date
                    DateTime? selectedDate = _datePicker1.SelectedDate;
                    if (selectedDate.HasValue)
                    {
                        datetime = selectedDate.Value;
                    }
                    //This timespan gets the time from the custom control and fills it with the hours, minutes and seconds filled in by the user.
                    TimeSpan time = new TimeSpan(startTime.Value.Hours, startTime.Value.Minutes, startTime.Value.Seconds);
                    // Finally, the time gets added to the full datetime
                    datetime = datetime.Date + time;

                    currentApp.tournament.TimeAndDate = datetime;

                    var gameSets = new List<GameSet>() { new GameSet() { legs = new List<GameLeg>() {
                        new GameLeg() {
                            history=new List<Dictionary<Player, List<Triplet>>>(),
                            Scores=new Dictionary<Player,int>(),
                            Winner=null,
                            CurrentTurn=null
                        } } } };

                    // Setting the Game in the Tournament object
                    Game game = new Game() {
                        Winner = null,
                        setsAmount = Convert.ToInt32(amountOfSets.Text),
                        legsAmount = Convert.ToInt32(amountOfLegs.Text),
                        gameSets = gameSets
                    };
                    // Adding the Game to the game array in the global model.
                    currentApp.tournament.Games.Add(game);
                    // This tells App.xaml.cs to continue to the next window
                    DialogResult = true;
                    // Closes the Window
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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_datePicker1.SelectedDate != null) return;

            FieldInfo fiTextBox = typeof(DatePicker).GetField("_textBox", BindingFlags.Instance | BindingFlags.NonPublic);

            if (fiTextBox != null)
            {
                DatePickerTextBox dateTextBox =
                 (DatePickerTextBox)fiTextBox.GetValue(_datePicker1);

                if (dateTextBox != null)
                {
                    PropertyInfo piWatermark = dateTextBox.GetType().GetProperty("Watermark", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (piWatermark != null)
                    {
                        piWatermark.SetValue(dateTextBox, "...", null);
                    }
                }
            }
        }

    }
}
