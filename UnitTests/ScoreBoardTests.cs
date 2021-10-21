using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
        private Tournament context;
        private MainWindowViewModel viewModel;
        private List<Player> players;

        [TestInitialize]
        public void TestInitialize()
        {
            // the scores
            var score1 = new Score();
            score1.SetScore(GameType.NORMAL);
            score1 -= 501;
            var score2 = new Score();
            score2.SetScore(GameType.NORMAL);

            // The players 
            players = new List<Player> { 
                new Player { Name = "Henk", legsWon = 0, score = score1 }, 
                new Player { Name = "Piet", legsWon = 0, score = score2 } 
            };

            var gameLeg = new GameLeg
            {
                Winner = null,
            };

            var legs = new List<GameLeg>();

            legs.Add(gameLeg);

            var gameSet = new GameSet
            {
                legs = legs,
                Winner = null,
            };

            var sets = new List<GameSet>();

            var game = new Game
            {
                gameSets = sets,
                Winner = null,
            };

            
            //  3 GameLegs
            
            // var gameLeg2 = new GameLeg
            // {
            //     Winner = null,
            // };
            // var gameLeg3 = new GameLeg
            // {
            //     Winner = null,
            // };
            // Tournament
            //var tournament = new Tournament
            //{
            //    GamesToWin = 1,
            //    Players = players,
            //    Winner = null,
            //};

            viewModel = new MainWindowViewModel(players, gameLeg, gameSet, game);
        }

        [TestMethod]
        public void doesLegScoreIncreaseByOneIfMoreThanHalfOfLegsWon()
        {
            // Henk
            viewModel.checkIfLegWinner(players[0]);
            // Piet
            viewModel.checkIfLegWinner(players[1]);

            Assert.AreEqual("Henk", viewModel.gameLeg.Winner.Name);
            //Assert.AreEqual(0, players[1].legsWon);
        }

        [TestMethod]
        public void doesSetScoreIncreaseByOneIfMoreThanHalfOfSetsWon()
        {

        }
    }
}
