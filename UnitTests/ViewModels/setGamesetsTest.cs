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
    public class setGamesetsTest
    {
        private UserInputWindowViewModel viewmodel;
        [TestMethod]
        public void CheckIfPlayersAreNull()
        {
            viewmodel = new UserInputWindowViewModel();
            viewmodel.Players = new List<Player>();
            Assert.IsNotNull(viewmodel.Players);
        }

        [TestMethod]
        public void CheckIfDateIsNull()
        {
            viewmodel = new UserInputWindowViewModel();
            viewmodel.TournamentDateTime = new DateTime(2016, 4, 5);
            Assert.IsNotNull(viewmodel.TournamentDateTime);
        }

        [TestMethod]
        public void CheckIfTimeIsNull()
        {
            viewmodel = new UserInputWindowViewModel();
            viewmodel.TournamentTime = new TimeSpan(11, 11, 11);
            Assert.IsNotNull(viewmodel.TournamentTime);
        }

        [TestMethod]
        public void CheckIfAmountOfSetsIsNull()
        {
            viewmodel = new UserInputWindowViewModel();
            viewmodel.AmountOfSets = 3;
            Assert.IsNotNull(viewmodel.AmountOfSets);
        }

        [TestMethod]
        public void CheckIfAmountOfLegsIsNull()
        {
            viewmodel = new UserInputWindowViewModel();
            viewmodel.AmountOfLegs = 3;
            Assert.IsNotNull(viewmodel.AmountOfLegs);
        }
    }
}