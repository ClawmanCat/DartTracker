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
            var currentLeg = tournament.Games.Last().gameSets.Last().legs.Last(); // use Tournament View model to pass trough current legg
            string playerOne = tournament.Players[0].Name;
            string playerTwo = tournament.Players[1].Name;


            // Temporary data, remove me later!
            Throw t10 = new Throw(new NormalSegment(10, SegmentModifier.SINGLE));
            Throw t60 = new Throw(new NormalSegment(20, SegmentModifier.TRIPLE));
            Throw tbs = new Throw(new NamedSegment(NamedSegmentType.OUTSIDE_BOARD));

            currentLeg.p1History.Add(new Triplet(t10, t10, t60));
            currentLeg.p1History.Add(new Triplet(t10, t60, tbs));
            currentLeg.p2History.Add(new Triplet(t10, tbs, tbs));


            DataContext = new
            {
                leg = new GameLegViewModel(tournament.Players, currentLeg)
                // prefevrably use the tournament also this way; 
            };

            p1label.Text = playerOne;
            p2label.Text = playerTwo;

            // test(janek) 
            Player currentPlayer = currentApp.tournament.Players[0];
            GameSetViewModel main = new GameSetViewModel(0, null, null);

            // true
            bool a = main.loadTestData(5);
            // false
            bool b = main.loadTestData(6);
            // false
            bool c = main.loadTestData(7);
        }
    }
}
