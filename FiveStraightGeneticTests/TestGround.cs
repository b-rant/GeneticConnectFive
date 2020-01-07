using FiveStraightGame;
using FiveStraightGenetic;
using FiveStraightGenetic.Models;
using FiveStraightGenetic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiveStraightGeneticTests
{
    [TestClass]
    public class TestGround
    {

        [TestMethod]
        public void TestLocationValuator()
        {
            var chromosome = new Chromosome()
            {
                OffensiveMultiplyer = .5,
                DefensiveMultiplyer = .5
            };
            var locValuator = new LocationValuator(chromosome);

            var testRow = new List<int> { -1, 0, 0, 0, -1, 0, -1, -1, 0 };
            var testRow2 = new List<int> { -1, 0, 2, 2, 0, 1, 1, 1, 0 };

            //var result = locValuator.CalculateCombinedPlayerRowValue(testRow, 4, 1);
            //var result2 = locValuator.CalculateCombinedPlayerRowValue(testRow2, 4, 1);
            //Assert.IsNotNull(result);
        }

        /// <summary>
        /// { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        /// </summary>

        [TestMethod]
        public void TestLocationValuator2()
        {
            var chromosome = new Chromosome()
            {
                OffensiveMultiplyer = .7,
                DefensiveMultiplyer = .7
            };
            var locValuator = new LocationValuator(chromosome);

            var testRow = new List<int> { -1, 0, 0, 0, -1, 0, -1, -1, -1 };
            var testRow2 = new List<int> { -1, 0, 1, 0, -1, -1, 1, 0, -1 };

            var result1_0 = locValuator.CalculateCombinedPlayerRowValue(testRow, 4, 0);
            var result1_1 = locValuator.CalculateCombinedPlayerRowValue(testRow, 4, 1);
            var result2_0 = locValuator.CalculateCombinedPlayerRowValue(testRow2, 4, 0);
            var result2_1 = locValuator.CalculateCombinedPlayerRowValue(testRow2, 4, 1);
            //Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestLocationValuator3()
        {
            var chromosome = new Chromosome()
            {
                OffensiveMultiplyer = .7,
                DefensiveMultiplyer = .7
            };
            var locValuator = new LocationValuator(chromosome);

            var game = new Game();
            var values = new List<double>();

            foreach (var spot in game.Board)
            {
                values.Add(locValuator.CalculateLocationValue(game.Board, spot.Number, 0));
            }

            //var result2_1 = locValuator.CalculateCombinedPlayerRowValue(testRow2, 4, 1);
            Assert.IsNotNull(values);
        }

        [TestMethod]
        public void TestLengthofGenerationRun()
        {
            var size = Configuration.GenerationSize();

            List<Chromosome> inputGeneration = new List<Chromosome>();
            for (int i = 0; i < Configuration.GenerationSize(); i++)
            {
                inputGeneration.Add(new Chromosome());
            }

            GeneticManager geneticManager = new GeneticManager();

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            inputGeneration = geneticManager.ProcessGeneration(inputGeneration);

            watch.Stop();

            var time = watch.ElapsedMilliseconds;

            Assert.AreEqual(Configuration.GenerationSize(), inputGeneration.Count);
        }

        [TestMethod]
        public void RunTrainingTest()
        {
            var size = Configuration.GenerationSize();

            List<Chromosome> inputGeneration = new List<Chromosome>();
            for (int i = 0; i < Configuration.GenerationSize(); i++)
            {
                inputGeneration.Add(new Chromosome());
            }

            GeneticManager geneticManager = new GeneticManager();

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            int rounds = 100;
            while (rounds != 0)
            {
                inputGeneration = geneticManager.ProcessGeneration(inputGeneration);
                rounds--;
            }

            watch.Stop();

            var time = watch.ElapsedMilliseconds;

            Assert.AreEqual(Configuration.GenerationSize(), inputGeneration.Count);
        }


        [TestMethod]
        public void TestFulLocationValuation()
        {
            var chromosome = new Chromosome()
            {
                OffensiveMultiplyer = .5,
                DefensiveMultiplyer = .5
            };
            var locValuator = new LocationValuator(chromosome);

            var game = GenerateTestInProgressGame();


            var unFilledboardLocations = game.Board.Where(x => !x.Filled);
            Dictionary<int, double> BoardValueResults = new Dictionary<int, double>();

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            foreach (var loc in unFilledboardLocations)
            {
                BoardValueResults.Add(loc.Number, locValuator.CalculateLocationValue(game.Board, loc.Number, 1));
            }

            watch.Stop();

            var time = watch.ElapsedMilliseconds;
        }

        public Game GenerateTestInProgressGame()
        {
            var game = new Game();

            for (int i = 0; i < 40; i++)
            {
                var player = game.GetPlayerByNumber(i % 2);

                game.PlayLocation(player, player.Hand.FirstOrDefault(), player.Hand.FirstOrDefault());

                var player2 = game.GetPlayerByNumber((i + 1) % 2);

                game.PlayLocation(player2, player2.Hand.FirstOrDefault(), player2.Hand.FirstOrDefault());

                game.PlayDrawCard(player);
                game.PlayDrawCard(player2);
            }

            return game;
        }
    }
}
