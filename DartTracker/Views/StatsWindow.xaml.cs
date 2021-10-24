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
using DartTracker.ViewModels;

namespace DartTracker.Views
{
    /// <summary>
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        private StatsWindowViewModel _statsWindowViewModel;
        public StatsWindow(Tournament tournament)
        {
            InitializeComponent();
            _statsWindowViewModel = new StatsWindowViewModel(tournament);
            DataContext = _statsWindowViewModel;

            SetSelectionBox.ItemsSource = _statsWindowViewModel.Sets;
            SetSelectionBox.ItemsSource = _statsWindowViewModel.Sets;
            LegSelectionBox.ItemsSource = _statsWindowViewModel.Legs;
            LegSelectionBox.ItemsSource = _statsWindowViewModel.Legs;
            
        }

        private void SetSelectionBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = LegSelectionBox.SelectedIndex;
            _statsWindowViewModel.SetNewSet(index);
        }

        private void LegSelectionBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = LegSelectionBox.SelectedIndex;
            _statsWindowViewModel.SetNewLeg(index);
        }
    }
}
