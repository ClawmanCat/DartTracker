using DartTracker.Models;
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
        public Tournament mainWindowTournamentObject;
        public MainWindow(Tournament tournament)
        {
            InitializeComponent();
            mainWindowTournamentObject = tournament;
            Closed += MainWindow_Closed;
            setBindings();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        public void setBindings()
        {
            string playerOne = mainWindowTournamentObject.Players[0].Name;
            string playerTwo = mainWindowTournamentObject.Players[1].Name;

            p1label.Text = playerOne;
            p2label.Text = playerTwo;
        }
    }
}
