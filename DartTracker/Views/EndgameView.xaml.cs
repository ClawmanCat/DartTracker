using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private bool check = false;
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
            Loaded += EndgameView_Loaded;
            Closed += EndgameView_Closed;
        }

        private void EndgameView_Closed(object sender, EventArgs e)
        {
            if(check == true)
            {
                EndGame();
            }
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
        public string Serialize()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.Auto;
            string jsonString = JsonConvert.SerializeObject(currentApp.tournament, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                });
            return jsonString;
        }
        public void SaveJson()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON-Formatted Data (*json)|*.json";

            if (dialog.ShowDialog() ?? false)
            {
                string jsonString = Serialize();
                File.WriteAllText(dialog.FileName, jsonString, Encoding.UTF8);
            }
        }

        public void EndGame()
        {
            SaveJson();
            currentApp.StartNewTournament();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            check = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            SaveJson();
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
