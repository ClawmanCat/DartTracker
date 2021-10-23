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
        // stats per set 
        // stats per leg 
        // stats total 


        public StatsWindowViewModel(Tournament tournament)
        {
            _tournament = tournament;
            // var average = GetAverageInGame();
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

        public double CalculateAverageScoreInTurn(ObservableCollection<Triplet> turns)
        {
            List<int> throwScores = new List<int>();

            foreach (var triplet in turns)
                foreach (var trow in triplet.throws)
                    throwScores.Add(trow.segment.Score);

            return throwScores.Average();
        }
        private Dictionary<string, Tuple<double, int>> WeightedMeans(List<Dictionary<string, Tuple<double, int>>> combinedHistory)
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

        public Dictionary<string,int> GetNumberOf180SInGame(Game game)
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
    }
}