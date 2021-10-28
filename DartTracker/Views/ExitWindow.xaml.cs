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
using System.Windows.Shapes;
using DartTracker.Models;

namespace DartTracker.Views
{
    /// <summary>
    /// Interaction logic for ExitWindow.xaml
    /// </summary>
    public partial class ExitWindow : Window
    {
        public App currentApp = Application.Current as App;
        public ExitWindow()
        {
            InitializeComponent();
            // Closed += ExitWindow_Closed;
            WinnerLabel.Text = "henk";
        }
        // private void ExitWindow_Closed(object sender, EventArgs e)
        // {
        //     App.Current.Shutdown();
        // }

        private void ButtonBase_OnClick_Quit(object sender, RoutedEventArgs e)
        {
           //Close();
        }

        private void ButtonBase_OnClick_NewGame(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
