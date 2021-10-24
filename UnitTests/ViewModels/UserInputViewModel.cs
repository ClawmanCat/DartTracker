using Microsoft.VisualStudio.TestTools.UnitTesting;
using DartTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartTracker.Models;

namespace DartTracker.Tests
{
    [TestClass()]
    public class UserInputViewModel
    {
        [TestMethod()]
        public void updateModelDateTest()
        {
            Score score = new Score();
            score.SetScore(GameType.LONG);
            Tournament tournament = new Tournament();
            tournament.GamesToWin = 1;
            tournament.Games = new List<Game>(1);
            tournament.Players = new List<Player>(2) { new Player(), new Player() };
            tournament.TimeAndDate = new DateTime(2021, 10, 23);
            DateTime newdate = new DateTime(2021, 10, 29);
            tournament.Winner = null;
            UserInputWindowViewModel vm = new UserInputWindowViewModel(tournament,score);
            vm.updateModelDate(tournament, newdate);
            Assert.AreEqual(tournament.TimeAndDate, newdate);
        }
        [TestMethod()]
        public void updateModelTimeTest()
        {
            Score score = new Score();
            score.SetScore(GameType.LONG);
            Tournament tournament = new Tournament();
            tournament.GamesToWin = 1;
            tournament.Games = new List<Game>(1);
            tournament.Players = new List<Player>(2) { new Player(), new Player() };
            tournament.Time = new TimeSpan(12, 10, 00);
            TimeSpan newdate = new TimeSpan(12, 10, 29);
            tournament.Winner = null;
            UserInputWindowViewModel vm = new UserInputWindowViewModel(tournament, score);
            vm.updateModelTime(tournament, newdate);
            Assert.AreEqual(tournament.Time, newdate);
        }

        [TestMethod()]
        public void createViewModelTest()
        {
            Score score = new Score();
            Tournament tournament = new Tournament();
            UserInputWindowViewModel vm = new UserInputWindowViewModel(tournament, score);
            Assert.IsNotNull(vm);
        }

        [TestMethod()]
        public void GameCreationTest1()
        {
            string name1 = "Henk";
            string name2 = "Piet";
            List<Player> playerlist = new List<Player>() { new Player { Name = name1 }, new Player { Name = name2} };

            List<Player> playerslist1 = new List<Player>() { new Player { Name = name1 }, new Player { Name = name1 } };
            Score score = new Score();
            Tournament tournament = new Tournament();
            tournament.Games = new List<Game>(1);
            UserInputWindowViewModel vm = new UserInputWindowViewModel(tournament, score);
            tournament.Players = playerlist;
            vm.AmountOfSets = 3;
            vm.AmountOfLegs = 4;
            bool createGameSets = true;
            vm.setGamesets(createGameSets);
            Assert.IsNotNull(tournament.Games);
        }
        [TestMethod()]
        public void GameCreationTest2()
        {
            string name1 = "Henk";
            string name2 = "Henk";
            List<Player> playerlist = new List<Player>() { new Player { Name = name1 }, new Player { Name = name2 } };
            Score score = new Score();
            Tournament tournament = new Tournament();
            tournament.Games = new List<Game>(1);
            UserInputWindowViewModel vm = new UserInputWindowViewModel(tournament, score);
            tournament.Players = playerlist;
            vm.AmountOfSets = 3;
            vm.AmountOfLegs = 4;
            bool createGamesets = false;
            vm.setGamesets(createGamesets);
            Assert.AreEqual(tournament.Games.Count, 0);
        }
    }
}