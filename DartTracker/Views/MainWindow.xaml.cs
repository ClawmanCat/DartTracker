﻿using DartTracker.Models;
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
using DartTracker.Views;

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
            List<Score> scores = currentApp.scores;
            string playerOne = tournament.Players[0].Name;
            string playerTwo = tournament.Players[1].Name;



            DataContext = new
            {
                leg = new MainWindowViewModel(tournament.Players, tournament.Games.Last(), scores),
                tournament = new TournamentViewModel(tournament),
                // preferably use the tournament also this way; 
            };

            p1label.Text = playerOne;
            p2label.Text = playerTwo;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            StatsWindow statsWindow = new StatsWindow(currentApp.tournament);
            statsWindow.Show();
        }
    }
}
