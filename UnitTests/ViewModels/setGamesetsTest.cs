using Microsoft.VisualStudio.TestTools.UnitTesting;
using DartTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartTracker.Tests
{
    [TestClass()]
    public class setGamesetsTest
    {
        private UserInputWindowViewModel viewmodel;
        [TestMethod()]
        public void SetGamesetsTest()
        {
            viewmodel = new UserInputWindowViewModel();
            Assert.IsNotNull(viewmodel);
        }

        [TestMethod]
        public void CheckIfPlayersAreNull()
        {
            viewmodel = new UserInputWindowViewModel();
            
            Assert.IsNotNull(viewmodel.Players);
        }

        [TestMethod]
        public void CheckIfDateIsNull()
        {
            viewmodel = new UserInputWindowViewModel();

            Assert.IsNotNull(viewmodel.TournamentDateTime);
        }

        [TestMethod]
        public void CheckIfTimeIsNull()
        {
            viewmodel = new UserInputWindowViewModel();

            Assert.IsNotNull(viewmodel.TournamentTime);
        }

        [TestMethod]
        public void CheckIfAmountOfSetsIsNull()
        {
            viewmodel = new UserInputWindowViewModel();

            Assert.IsNotNull(viewmodel.AmountOfSets);
        }

        [TestMethod]
        public void CheckIfAmountOfLegsIsNull()
        {
            viewmodel = new UserInputWindowViewModel();

            Assert.IsNotNull(viewmodel.AmountOfLegs);
        }
    }
}