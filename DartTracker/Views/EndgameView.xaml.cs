using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DartTracker.Views
{
    /// <summary>
    /// Interaction logic for EndgameView.xaml
    /// </summary>
    public partial class EndgameView : Window
    {
        public App currentApp = Application.Current as App;
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public EndgameView()
        {
            InitializeComponent();
            string message = string.Format("{0} heeft gewonnen! \nWil je een nieuw spel starten?", currentApp.tournament.Games.First().Winner.Name);
            introText.Text = message;
            Closed += EndgameView_Closed;
            Loaded += EndgameView_Loaded;
        }

        /// <summary>
        /// Removing the X from the window during the load process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndgameView_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void EndgameView_Closed(object sender, EventArgs e)
        {
            currentApp.StartNewTournament();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void StatsButton_Click(object sender, RoutedEventArgs e)
        {
            StatsWindow statsWindow = new StatsWindow(currentApp.tournament);
            statsWindow.Owner = this;
            statsWindow.ShowDialog();
            statsWindow = null;
        }
    }
}
