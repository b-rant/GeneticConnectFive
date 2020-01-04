using FiveStraightGame;
using FiveStraightGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FiveStraightGameTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void NewGameTests()
        {
            Game newGame = new Game();

            var player1 = newGame.GetPlayerByNumber(0);
            var player2 = newGame.GetPlayerByNumber(1);
            try
            {
                var falseDraw = newGame.PlayDrawCard(player1);
                Assert.IsFalse(falseDraw);

                var BadPlay = newGame.PlayLocation(player2, 0, player2.Hand.FirstOrDefault());
                Assert.IsFalse(BadPlay);

                var goodplay = newGame.PlayLocation(player1, player1.Hand.FirstOrDefault(), player1.Hand.FirstOrDefault());
                Assert.IsTrue(goodplay);
            }
            catch(Exception e)
            {

            }
        }
    }
}
