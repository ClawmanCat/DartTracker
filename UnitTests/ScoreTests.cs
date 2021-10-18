using Microsoft.VisualStudio.TestTools.UnitTesting;
using DartTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass()]
    public class ScoreTests
    {
        [TestMethod()]
        public void SubstractScoreTest01()
        {
            //Arrange
            Score score = new Score();
            score.SetScore(GameType.SHORT);
            //Act
            score -= 60;
            //Assert
            Assert.AreEqual(241, (int)score);
        }

        [TestMethod()]
        public void SubstractScoreTest02()
        {
            //Arrange
            Score score = new Score();
            score.SetScore(GameType.SHORT);
            //Act
            score -= 180;
            //Assert
            Assert.AreEqual(121, (int)score);
        }

        [TestMethod()]
        public void SubstractScoreTest03()
        {
            //Arrange
            Score score = new Score();
            score.SetScore(GameType.NORMAL);
            //Act
            score -= 60;
            //Assert
            Assert.AreEqual(441, (int)score);
        }

        [TestMethod()]
        public void SubstractScoreTest04()
        {
            //Arrange
            Score score = new Score();
            score.SetScore(GameType.NORMAL);
            //Act
            score -= 180;
            //Assert
            Assert.AreEqual(321, (int)score);
        }

        [TestMethod()]
        public void SubstractScoreTest05()
        {
            //Arrange
            Score score = new Score();
            score.SetScore(GameType.LONG);
            //Act
            score -= 60;
            //Assert
            Assert.AreEqual(641, (int)score);
        }

        [TestMethod()]
        public void SubstractScoreTest06()
        {
            //Arrange
            Score score = new Score();
            score.SetScore(GameType.LONG);
            //Act
            score -= 180;
            //Assert
            Assert.AreEqual(521, (int)score);
        }
    }
}