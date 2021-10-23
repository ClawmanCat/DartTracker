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
    class StatsWindowViewModel
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
            var average = GetAverageInGame();
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

        private Dictionary<string, double> GetAverageInGame()
        {
            Dictionary<string, double> averageScoreDictionary = new Dictionary<string, double>();
            foreach (var set in _gameHistory)
            {
                var averageScoreInSet = CalculateAverageScoreInSet(set);
                foreach (var setAverages in averageScoreDictionary)
                    averageScoreDictionary.Add(setAverages.Key, setAverages.Value);
            }

            return averageScoreDictionary;
        }

        private Dictionary<string, double> CalculateAverageScoreInSet(
            List<Dictionary<string, ObservableCollection<Triplet>>> set)
        {
            Dictionary<string, double> averageScoreDictionary = new Dictionary<string, double>();
            foreach (var leg in set)
            {
                var legScores = CalculateAverageScoreInLeg(leg);
                foreach (var scores in legScores)
                    averageScoreDictionary.Add(scores.Key, scores.Value);
            }

            return averageScoreDictionary;
        }

        private Dictionary<string, double> CalculateAverageScoreInLeg(
            Dictionary<string, ObservableCollection<Triplet>> leg)
        {
            Dictionary<string, double> averageScoreDictionary = new Dictionary<string, double>();
            foreach (var playerTurns in leg)
                averageScoreDictionary.Add(playerTurns.Key, CalculateAverageScoreInTurn(playerTurns.Value));
                
            return averageScoreDictionary;
        }

        private double CalculateAverageScoreInTurn(ObservableCollection<Triplet> turns)
        {
            List<int> throwScores = new List<int>();

            foreach (var triplet in turns)
                foreach (var trow in triplet.throws)
                    throwScores.Add(trow.segment.Score);

            return throwScores.Average();
        }
    }
}