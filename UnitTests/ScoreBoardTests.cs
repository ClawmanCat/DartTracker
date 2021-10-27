using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartTracker.Models;
using DartTracker.ViewModels;

namespace UnitTests
{
    [TestClass]
    public class ScoreBoardTests
    {
        private MainWindowViewModel viewModel;
        private List<Player> players;
        private GameLeg gameLeg;
        private GameLeg gameLeg2;
        private GameSet gameSet;
        private Score score1;
        private Game game;

        [TestInitialize]
        public void TestInitialize()
        {
            // the scores
            score1 = new Score();
            score1.SetScore(GameType.NORMAL);
            score1 -= 501;
            var score2 = new Score();
            score2.SetScore(GameType.NORMAL);

            // The players 
            players = new List<Player>
            {
                new Player {Name = "Henk", legsWon = 0, score = score1},
                new Player {Name = "Piet", legsWon = 0, score = score2}
            };
        }

        private MainWindowViewModel createTestCode(int setsAmount, int legsAmount)
        {
            Score score = new Score();
            score.SetScore(GameType.NORMAL);
            score -= 501;


            var legs = new List<GameLeg>()
            {
                new GameLeg()
                {
                    Winner = players[0],
                    CurrentTurn = new Player {Name = "Henk"},
                    history = new Dictionary<string, ObservableCollection<Triplet>>()
                    {
                        {
                            "Henk",
                            new ObservableCollection<Triplet>() { }
                        },
                        {
                            "Piet",
                            new ObservableCollection<Triplet>() { }
                        }
                    },
                    ScoreHistory = new Dictionary<string, ObservableCollection<int>>()
                    {
                        {
                            "Henk",
                            new ObservableCollection<int>() { }
                        },
                        {
                            "Piet",
                            new ObservableCollection<int>() { }
                        }
                    }
                },
                new GameLeg()
                {
                    Winner = players[0],
                    CurrentTurn = new Player {Name = "Henk"},
                    history = new Dictionary<string, ObservableCollection<Triplet>>()
                    {
                        {
                            "Henk",
                            new ObservableCollection<Triplet> { }
                        },
                        {
                            "Piet",
                            new ObservableCollection<Triplet> { }
                        }
                    },
                    ScoreHistory = new Dictionary<string, ObservableCollection<int>>()
                    {
                        {
                            "Henk",
                            new ObservableCollection<int>() { }
                        },
                        {
                            "Piet",
                            new ObservableCollection<int>() { }
                        }
                    }
                }
            };

            gameSet = new GameSet
            {
                legs = legs,
                Winner = null,
            };

            var sets = new List<GameSet>();

            sets.Add(gameSet);

            game = new Game
            {
                gameSets = sets,
                Winner = null,
                setsAmount = setsAmount,
                legsAmount = legsAmount,
            };
            return new MainWindowViewModel(players, game, score);
        }

        [TestMethod]
        public void doesLegScoreIncreaseByOneIfMoreThanHalfOfLegsWon()
        {
            // 5 legs, 5 sets
            viewModel = createTestCode(5, 5);

            //Henk
            viewModel.checkWinner(players[0]);

            // Piet
            viewModel.checkWinner(players[1]);
            Assert.AreEqual(2, players[0].legsWon);
            Assert.AreEqual(0, players[1].legsWon);
        }

        [TestMethod]
        public void doesSetScoreIncreaseByOneIfMoreThanHalfOfSetsWon()
        {
            // 3 legs, 3 sets
            viewModel = createTestCode(3, 3);

            // Henk
            viewModel.checkWinner(players[0]);

            // Piet
            viewModel.checkWinner(players[1]);
            Assert.AreEqual(1, players[0].setsWon);
            Assert.AreEqual(0, players[1].setsWon);
        }
    }
}