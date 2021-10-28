using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartTracker.Models;

namespace DartTracker.Utility
{
    public class AveragesScores
    {
        private static AveragesScores instance = null;

        private AveragesScores()
        {
        }

        public static AveragesScores Instantance
        {
            get
            {
                if (instance == null)
                    instance = new AveragesScores();
                return instance;
            }
        }

        public Dictionary<int, Tuple<double, int>> CalculateAverageScoreInGame(Game game)
        {
            Dictionary<int, Tuple<double, int>>
                averageScoreDictionary = new Dictionary<int, Tuple<double, int>>();
            List<Dictionary<int, Tuple<double, int>>>
                allSets = new List<Dictionary<int, Tuple<double, int>>>();
            foreach (var set in game.gameSets)
                allSets.Add(CalculateAverageScoreInSet(set));

            return WeightedMeans(allSets);
        }

        public Dictionary<int, Tuple<double, int>> CalculateAverageScoreInSet(GameSet set)
        {
            Dictionary<int, Tuple<double, int>>
                averageScoreDictionary = new Dictionary<int, Tuple<double, int>>();
            List<Dictionary<int, Tuple<double, int>>>
                allLegs = new List<Dictionary<int, Tuple<double, int>>>();
            foreach (var leg in set.legs)
                allLegs.Add(CalculateAverageScoreInLeg(leg.history));

            return WeightedMeans(allLegs);
        }

        public Dictionary<int, Tuple<double, int>> CalculateAverageScoreInLeg(
            Dictionary<int, ObservableCollection<Triplet>> leg)
        {
            Dictionary<int, Tuple<double, int>>
                averageScoreDictionary = new Dictionary<int, Tuple<double, int>>();
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

        private Dictionary<int, Tuple<double, int>> WeightedMeans(
            List<Dictionary<int, Tuple<double, int>>> combinedHistory)
        {
            Dictionary<int, Tuple<double, int>>
                combinedMeanDictionary = new Dictionary<int, Tuple<double, int>>();
            if (combinedMeanDictionary == null) throw new ArgumentNullException(nameof(combinedMeanDictionary));

            Dictionary<int, Tuple<double, int>> weightAndValueSum =
                new Dictionary<int, Tuple<double, int>>();

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
            {
                double averageWeights = player.Value.Item1 / player.Value.Item2;
                if (double.IsNaN(averageWeights))
                    averageWeights = 0;
                combinedMeanDictionary[player.Key] = new Tuple<double, int>(
                   averageWeights, player.Value.Item2
                );
            }


            return combinedMeanDictionary;
        }
    }

}