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
            NumberOfSetsWonPlayerOne.Content = tournament.Players[0].TotalSetsWon;
            NumberOfLegsWonPlayerOne.Content = tournament.Players[0].TotalLegsWon;
            NumberOfSetsWonPlayerTwo.Content = tournament.Players[1].TotalSetsWon;
            NumberOfLegsWonPlayerTwo.Content = tournament.Players[1].TotalLegsWon;
            SetSelectionBox.SelectedItem = SetSelectionBox.Items[0];
            LegSelectionBox.SelectedItem = LegSelectionBox.Items[0];

        }

        

        private void SetSelectionBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // var x =LegSelectionBox.SelectedValue;
            // if ((string) LegSelectionBox.SelectionBoxItem == "")
            //     return;
            // var index = (LegSelectionBox.SelectedItem.ToString() ?? string.Empty).Last() - '0';
            //
            var index = SetSelectionBox.SelectedIndex;
            if (index == -1)
                return;
            _statsWindowViewModel.SetNewSet(index);
        }

        private void LegSelectionBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = LegSelectionBox.SelectedIndex;
            if(index == -1)
                return;
            
            _statsWindowViewModel.SetNewLeg(index);
        }
    }
}
