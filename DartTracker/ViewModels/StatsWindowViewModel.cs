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

namespace DartTracker.ViewModels
{
    public class StatsWindowViewModel : INotifyPropertyChanged
    {
        // public Dictionary<string, ObservableCollection<Triplet>> FirstHistory => _totalHistory.First();


        private readonly Tournament _tournament;
        private GameLeg _gameLeg;

        public GameLeg CurrentLeg
        {
            get { return _gameLeg; }
            set
            {
                _gameLeg = value;
                OnPropertyChanged("CurrentLeg");
            }
        }


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
        public double AverageInSetPlayerOne {
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


        public GameSet CurrentSet { get; set; }
        public Game CurrentGame { get; set; }

        public List<string> Sets { get; set; }
        public List<string> Legs { get; set; }


        public StatsWindowViewModel(Tournament tournament)
        {
            Sets = new List<string>();
            Legs = new List<string>();
            _tournament = tournament;
            CurrentGame = tournament.Games.First();
            CurrentSet = CurrentGame.gameSets.First();
            CurrentLeg = CurrentSet.legs.First();
            FillComboBoxes();
            SetAverages();
           
        }

        private void SetAverages()
        {
            var averagesScoresInGame = CalculateAverageScoreInGame(CurrentGame);
            AverageInGamePlayerOne = averagesScoresInGame.ToArray()[0].Value.Item1;
            AverageInGamePlayerTwo = averagesScoresInGame.ToArray()[1].Value.Item1;
            var averagesScoresInSet = CalculateAverageScoreInSet(CurrentSet);
            AverageInSetPlayerOne = averagesScoresInSet.ToArray()[0].Value.Item1;
            AverageInSetPlayerTwo = averagesScoresInSet.ToArray()[1].Value.Item1;
            var averagesScoresInLeg = CalculateAverageScoreInLeg(CurrentLeg.history);
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


        public Dictionary<string, Tuple<double, int>> CalculateAverageScoreInGame(Game game)
        {
            Dictionary<string, Tuple<double, int>>
                averageScoreDictionary = new Dictionary<string, Tuple<double, int>>();
            List<Dictionary<string, Tuple<double, int>>>
                allSets = new List<Dictionary<string, Tuple<double, int>>>();
            foreach (var set in game.gameSets)
                allSets.Add(CalculateAverageScoreInSet(set));

            return WeightedMeans(allSets);
        }

        public Dictionary<string, Tuple<double, int>> CalculateAverageScoreInSet(GameSet set)
        {
            Dictionary<string, Tuple<double, int>>
                averageScoreDictionary = new Dictionary<string, Tuple<double, int>>();
            List<Dictionary<string, Tuple<double, int>>>
                allLegs = new List<Dictionary<string, Tuple<double, int>>>();
            foreach (var leg in set.legs)
                allLegs.Add(CalculateAverageScoreInLeg(leg.history));

            return WeightedMeans(allLegs);
        }

        public Dictionary<string, Tuple<double, int>> CalculateAverageScoreInLeg(
            Dictionary<string, ObservableCollection<Triplet>> leg)
        {
            Dictionary<string, Tuple<double, int>>
                averageScoreDictionary = new Dictionary<string, Tuple<double, int>>();
            foreach (var playerTurns in leg)
            {
                if (playerTurns.Value.Count == 0)
                {
                    averageScoreDictionary[playerTurns.Key] = Tuple.Create(0d, 0);
                    continue;
                }
                averageScoreDictionary[playerTurns.Key] = Tuple.Create(CalculateAverageScoreInTurn(playerTurns.Value),
                    playerTurns.Value.Count * 3);
            }
                

            return averageScoreDictionary;
        }

        public double CalculateAverageScoreInTurn(ObservableCollection<Triplet> turns)
        {
            List<int> throwScores = new List<int>();

            foreach (var triplet in turns)
            foreach (var trow in triplet.throws)
                throwScores.Add(trow.segment.Score);

            return throwScores.Average();
        }

        private Dictionary<string, Tuple<double, int>> WeightedMeans(
            List<Dictionary<string, Tuple<double, int>>> combinedHistory)
        {
            Dictionary<string, Tuple<double, int>>
                combinedMeanDictionary = new Dictionary<string, Tuple<double, int>>();
            if (combinedMeanDictionary == null) throw new ArgumentNullException(nameof(combinedMeanDictionary));

            Dictionary<string, Tuple<double, int>> weightAndValueSum =
                new Dictionary<string, Tuple<double, int>>();

            foreach (var legs in combinedHistory)
            {
                foreach (var player in legs)
                {
                    double scoreValue = player.Value.Item1 * player.Value.Item2;
                    int weights = player.Value.Item2;
                    if (!weightAndValueSum.ContainsKey(player.Key))
                    {
                        weightAndValueSum[player.Key] = new Tuple<double, int>(scoreValue, weights);
                    }
                    else
                    {
                        var totalValueWeight = +weightAndValueSum[player.Key].Item1 + scoreValue;
                        int totalWeights = +weightAndValueSum[player.Key].Item2 + weights;
                        weightAndValueSum[player.Key] = new Tuple<double, int>(totalValueWeight, totalWeights);
                    }
                }
            }

            foreach (var player in weightAndValueSum)
                combinedMeanDictionary[player.Key] = new Tuple<double, int>(
                    player.Value.Item1 / player.Value.Item2, player.Value.Item2
                );

            return combinedMeanDictionary;
        }

        public Dictionary<string, int> GetNumberOf180SInGame(Game game)
        {
            Dictionary<string, int> dictOf180 = new Dictionary<string, int>();
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