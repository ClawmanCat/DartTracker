using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using DartTracker.Annotations;
using DartTracker.Models;
using DartTracker.Utility;

namespace DartTracker.ViewModels
{
    public class StatsWindowViewModel : INotifyPropertyChanged
    {

        private readonly Tournament _tournament;
        private GameLeg _gameLeg;

        private AveragesScores _averagesScores;
        public GameLeg CurrentLeg
        {
            get { return _gameLeg; }
            set
            {
                _gameLeg = value;
                OnPropertyChanged("CurrentLeg");
            }
        }

        #region PlayersAverages

        private double _averageInGamePlayerOne;
        private double _averageInSetPlayerOne;
        private double _averageInLegPlayerOne;

        private double _averageInGamePlayerTwo;
        private double _averageInSetPlayerTwo;
        private double _averageInLegPlayerTwo;

        public double AverageInGamePlayerOne
        {
            get { return _averageInGamePlayerOne; }
            set
            {
                _averageInGamePlayerOne = value;
                OnPropertyChanged("AverageInGamePlayerOne");
            }
        }
        public double AverageInSetPlayerOne
        {
            get { return _averageInSetPlayerOne; }
            set
            {
                _averageInSetPlayerOne = value;
                OnPropertyChanged("AverageInSetPlayerOne");
            }
        }
        public double AverageInLegPlayerOne
        {
            get { return _averageInLegPlayerOne; }
            set
            {
                _averageInLegPlayerOne = value;
                OnPropertyChanged("AverageInLegPlayerOne");
            }
        }

        public double AverageInGamePlayerTwo
        {
            get { return _averageInGamePlayerTwo; }
            set
            {
                _averageInGamePlayerTwo = value;
                OnPropertyChanged("AverageInGamePlayerTwo");
            }
        }
        public double AverageInSetPlayerTwo
        {
            get { return _averageInSetPlayerTwo; }
            set
            {
                _averageInSetPlayerTwo = value;
                OnPropertyChanged("AverageInSetPlayerTwo");
            }
        }
        public double AverageInLegPlayerTwo
        {
            get { return _averageInLegPlayerTwo; }
            set
            {
                _averageInLegPlayerTwo = value;
                OnPropertyChanged("AverageInLegPlayerTwo");
            }
        }

        #endregion

        public GameSet CurrentSet { get; set; }
        public Game CurrentGame { get; set; }

        public List<string> Sets { get; set; }
        public List<string> Legs { get; set; }


        public StatsWindowViewModel(Tournament tournament)
        {
            Sets = new List<string>();
            Legs = new List<string>();
            _averagesScores = AveragesScores.Instantance;
            _tournament = tournament;
            CurrentGame = tournament.Games.First();
            CurrentSet = CurrentGame.gameSets.First();
            CurrentLeg = CurrentSet.legs.First();
            FillComboBoxes();
            SetAverages();

        }

        private void SetAverages()
        {
            var averagesScoresInGame = _averagesScores.CalculateAverageScoreInGame(CurrentGame);
            AverageInGamePlayerOne = averagesScoresInGame.ToArray()[0].Value.Item1;
            AverageInGamePlayerTwo = averagesScoresInGame.ToArray()[1].Value.Item1;
            var averagesScoresInSet = _averagesScores.CalculateAverageScoreInSet(CurrentSet);
            AverageInSetPlayerOne = averagesScoresInSet.ToArray()[0].Value.Item1;
            AverageInSetPlayerTwo = averagesScoresInSet.ToArray()[1].Value.Item1;
            var averagesScoresInLeg = _averagesScores.CalculateAverageScoreInLeg(CurrentLeg.history);
            AverageInLegPlayerOne = averagesScoresInLeg.ToArray()[0].Value.Item1;
            AverageInLegPlayerTwo = averagesScoresInLeg.ToArray()[1].Value.Item1;
        }
        private void FillComboBoxes()
        {
            for (int i = 0; i < CurrentGame.gameSets.Count; i++)
                Sets.Add($"Set {i + 1}");
            for (int i = 0; i < CurrentSet.legs.Count; i++)
                Legs.Add($"Leg {i + 1}");
        }

        public Dictionary<int, int> GetNumberOf180SInGame(Game game)
        {
            Dictionary<int, int> dictOf180 = new Dictionary<int, int>();
            int total180S = 0;
            foreach (var set in game.gameSets)
                foreach (var leg in set.legs)
                    foreach (var player in leg.history)
                    {
                        if (!dictOf180.ContainsKey(player.Key))
                            dictOf180[player.Key] = Checked180S(player.Value);
                        else
                            dictOf180[player.Key] += Checked180S(player.Value);
                    }


            return dictOf180;
        }

        private int Checked180S(ObservableCollection<Triplet> leg)
        {
            int total180S = 0;
            foreach (var triplet in leg)
            {
                int tripleTwenties = 0;
                foreach (Throw trow in triplet.throws)
                {
                    if (trow.segment.Score == 60)
                        tripleTwenties++;
                    if (tripleTwenties == 3)
                        total180S++;
                }
            }

            return total180S;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetNewLeg(int index)
        {
            CurrentLeg = CurrentSet.legs[index];
            SetAverages();
        }

        public void SetNewSet(int index)
        {
            CurrentSet = CurrentGame.gameSets[index];
            CurrentLeg = CurrentSet.legs.First();
            SetAverages();
        }
    }
}