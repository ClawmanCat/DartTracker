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
            TimeSpan newdate = new TimeSpan(2021, 10, 29);
            tournament.Winner = null;
            UserInputWindowViewModel vm = new UserInputWindowViewModel(tournament, score);
            vm.updateModelTime(tournament, newdate);
            Assert.AreEqual(tournament.Time, newdate);
        }
    }
}