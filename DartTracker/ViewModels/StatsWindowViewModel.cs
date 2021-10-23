using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using DartTracker.Models;

namespace DartTracker.ViewModels
{
    public class StatsWindowViewModel
    {
        // public Dictionary<string, ObservableCollection<Triplet>> FirstHistory => _totalHistory.First();


        private readonly Tournament _tournament;

        private readonly List<List<Dictionary<string, ObservableCollection<Triplet>>>> _gameHistory =
            new List<List<Dictionary<string, ObservableCollection<Triplet>>>>();

        private List<Dictionary<string, ObservableCollection<Triplet>>> _totalHistory =
            new List<Dictionary<string, ObservableCollection<Triplet>>>();
        // stats per set 
        // stats per leg 
        // stats total 


        public StatsWindowViewModel(Tournament tournament)
        {
            _tournament = tournament;
            GetAllHistories();
            // var average = GetAverageInGame();
        }

        public void GetAllHistories()
        {
            List<GameSet> sets = _tournament.Games.First().gameSets; // if there are more games show current current games
            foreach (var set in sets)
            {
                var totalLegHistory = new List<Dictionary<string, ObservableCollection<Triplet>>>();
                foreach (var leg in set.legs)
                    totalLegHistory.Add(leg.history);

                _gameHistory.Add(totalLegHistory);
            }
        }

        public Dictionary<string, Tuple<double, int>> CalculateAverageScoreInGame(Game game)
        {
            Dictionary<string, Tuple<double,int>> averageScoreDictionary = new Dictionary<string, Tuple<double, int>>();
            List<Dictionary<string, Tuple<double, int>>>
                allSets = new List<Dictionary<string, Tuple<double, int>>>();
            foreach (var set in game.gameSets)
                allSets.Add(CalculateAverageScoreInSet(set));

            return WeightedMeans(allSets);
        }

        public Dictionary<string, Tuple<double,int>> CalculateAverageScoreInSet(GameSet set)
        {
            Dictionary<string, Tuple<double,int>> averageScoreDictionary = new Dictionary<string, Tuple<double, int>>();
            List<Dictionary<string, Tuple<double, int>>>
                allLegs = new List<Dictionary<string, Tuple<double, int>>>();
            foreach (var leg in set.legs)
                allLegs.Add(CalculateAverageScoreInLeg(leg.history));

            return WeightedMeans(allLegs);
        }

        public Dictionary<string, Tuple<double,int>> CalculateAverageScoreInLeg(
            Dictionary<string, ObservableCollection<Triplet>> leg)
        {
            Dictionary<string, Tuple<double,int>> averageScoreDictionary = new Dictionary<string, Tuple<double,int>>();
            foreach (var playerTurns in leg)
                averageScoreDictionary[playerTurns.Key] = Tuple.Create(CalculateAverageScoreInTurn(playerTurns.Value),playerTurns.Value.Count * 3);

            return averageScoreDictionary;
        }

        private Dictionary<string,Tuple<double,int>> WeightedMeans(List<Dictionary<string, Tuple<double, int>>> combinedHistory)
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
                    if(!weightAndValueSum.ContainsKey(player.Key))
                    {
                        weightAndValueSum[player.Key] = new Tuple<double, int>(scoreValue, weights);
                    }
                    else
                    {
                        var totalValueWeight =+ weightAndValueSum[player.Key].Item1 + scoreValue;
                        int totalWeights =+ weightAndValueSum[player.Key].Item2 + weights;
                        weightAndValueSum[player.Key] = new Tuple<double, int>(totalValueWeight, totalWeights);
                    }
                }
            }

            foreach (var player in weightAndValueSum)
                combinedMeanDictionary[player.Key] = new Tuple<double, int>(
                    player.Value.Item1/player.Value.Item2,player.Value.Item2
                    );

            return combinedMeanDictionary;
        }


        public double CalculateAverageScoreInTurn(ObservableCollection<Triplet> turns)
        {
            List<int> throwScores = new List<int>();

            foreach (var triplet in turns)
                foreach (var trow in triplet.throws)
                    throwScores.Add(trow.segment.Score);

            return throwScores.Average();
        }
    }
}