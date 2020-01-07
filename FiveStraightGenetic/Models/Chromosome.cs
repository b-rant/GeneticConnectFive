using FiveStraightGenetic.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FiveStraightGenetic.Models
{
    public class Chromosome : IComparer<Chromosome>
    {
        private Random Random;

        public Guid Id { get; set; }

        // Chromosome Properties (these are the changing values to effect gameplay choices)

        public double PotentialFiveMultiplyer { get; set; }

        public double OffensiveMultiplyer { get; set; }

        public double DefensiveMultiplyer { get; set; }

        public double DrawMultiplyer { get; set; }

        public double CardLocationDifferenceMultiplyer { get; set; }

        public double CardValueMultiplyer { get; set; }

        // Stats to track performace of Chromosome 

        public int NumberOfGenerationsSurvived { get; private set; }

        public int NumberOfGamesPlayed { get; private set; }

        public int Wins { get; private set; }

        public double SumOfGameLengthsInTurns { get; private set; }

        // Constructors for different generation needs

        /// <summary>
        /// Returns a fully random generated new Chromosome
        /// </summary>
        public Chromosome()
        {
            var guid = Guid.NewGuid();
            Random = new Random(guid.GetHashCode());
            Id = guid;
            MutatePotentialFiveMultiplyer();
            MutateOffensiveMultiplyer();
            MutateDefensiveMultiplyer();
            MutateDrawMultiplyer();
            MutateCardLocationDifferenceMultiplyer();
            MutateCardValueMultiplyer();
        }

        /// <summary>
        /// Returns a new Chromosome that is a copy, weight wise, of the provided chromosome
        /// </summary>
        /// <param name="toBeCoppied">Chromosome to copy</param>
        public Chromosome(Chromosome toBeCoppied)
        {
            var guid = Guid.NewGuid();
            Random = new Random(guid.GetHashCode());
            Id = guid;
            NumberOfGenerationsSurvived = ++toBeCoppied.NumberOfGenerationsSurvived;
            PotentialFiveMultiplyer = toBeCoppied.PotentialFiveMultiplyer;
            OffensiveMultiplyer = toBeCoppied.OffensiveMultiplyer;
            DefensiveMultiplyer = toBeCoppied.DefensiveMultiplyer;
            DrawMultiplyer = toBeCoppied.DrawMultiplyer;
            CardLocationDifferenceMultiplyer = toBeCoppied.CardLocationDifferenceMultiplyer;
            CardValueMultiplyer = toBeCoppied.CardValueMultiplyer;
        }

        /// <summary>
        /// Returns a new Chromosome that is a combination of values in the two provided chromosomes
        /// </summary>
        /// <param name="crossoverA">First Chromosome</param>
        /// <param name="crossoverB">Second Chromosome</param>
        public Chromosome(Chromosome crossoverA, Chromosome crossoverB)
        {
            var guid = Guid.NewGuid();
            Random = new Random(guid.GetHashCode());
            Id = guid;
            if (Random.NextDouble() >= .5)
                PotentialFiveMultiplyer = crossoverA.PotentialFiveMultiplyer;
            else
                PotentialFiveMultiplyer = crossoverB.PotentialFiveMultiplyer;

            if (Random.NextDouble() >= .5)
                OffensiveMultiplyer = crossoverA.OffensiveMultiplyer;
            else
                OffensiveMultiplyer = crossoverB.OffensiveMultiplyer;

            if (Random.NextDouble() >= .5)
                DefensiveMultiplyer = crossoverA.DefensiveMultiplyer;
            else
                DefensiveMultiplyer = crossoverB.DefensiveMultiplyer;

            if (Random.NextDouble() >= .5)
                DrawMultiplyer = crossoverA.DrawMultiplyer;
            else
                DrawMultiplyer = crossoverB.DrawMultiplyer;

            if (Random.NextDouble() >= .5)
                CardLocationDifferenceMultiplyer = crossoverA.CardLocationDifferenceMultiplyer;
            else
                CardLocationDifferenceMultiplyer = crossoverB.CardLocationDifferenceMultiplyer;

            if (Random.NextDouble() >= .5)
                CardValueMultiplyer = crossoverA.CardValueMultiplyer;
            else
                CardValueMultiplyer = crossoverB.CardValueMultiplyer;
        }

        /// <summary>
        /// This function will randomly mutate properties on the chromosome
        /// </summary>
        public void Mutate()
        {
            if (GetRandomNumber(0,1) <= Configuration._MutationRate)
            {
                MutatePotentialFiveMultiplyer();
            }
            if (GetRandomNumber(0, 1) <= Configuration._MutationRate)
            {
                MutateOffensiveMultiplyer();
            }
            if (GetRandomNumber(0, 1) <= Configuration._MutationRate)
            {
                MutateDefensiveMultiplyer();
            }
            if (GetRandomNumber(0, 1) <= Configuration._MutationRate)
            {
                MutateDrawMultiplyer();
            }
            if (GetRandomNumber(0, 1) <= Configuration._MutationRate)
            {
                MutateCardLocationDifferenceMultiplyer();
            }
            if (GetRandomNumber(0, 1) <= Configuration._MutationRate)
            {
                MutateCardValueMultiplyer();
            }
        }

        /// <summary>
        /// Processes a given match, updating the score of the chromosome
        /// </summary>
        /// <param name="match"></param>
        public void ProcessMatch(Match match)
        {
            NumberOfGamesPlayed++;
            if (match.WinningPlayer == Id)
            {
                Wins++;
            }
            SumOfGameLengthsInTurns += match.GameLengthInTurns;
        }

        /// <summary>
        /// Returns the average Game Length in turns of the Chromosome
        /// </summary>
        /// <returns></returns>
        public double GameLegthInTurnsAverage()
        {
            if (NumberOfGamesPlayed == 0)
            {
                return 0;
            }
            return (double)SumOfGameLengthsInTurns/NumberOfGamesPlayed;
        }

        /// <summary>
        /// Returns the Value of a chromosome based on win average
        /// </summary>
        /// <returns></returns>
        public double GetChromosomeRankValue()
        {
            if (NumberOfGamesPlayed == 0)
            {
                return 0;
            }
            return (double)Wins/NumberOfGamesPlayed;
        }

        // Private functions

        private void MutatePotentialFiveMultiplyer()
        {
            PotentialFiveMultiplyer = GetRandomNumber(5, 25);
        }

        private void MutateOffensiveMultiplyer()
        {
            OffensiveMultiplyer = GetRandomNumber(.1, 1);
        }

        private void MutateDefensiveMultiplyer()
        {
            DefensiveMultiplyer = GetRandomNumber(.1, 1);
        }

        private void MutateDrawMultiplyer()
        {
            DrawMultiplyer = GetRandomNumber(9, 20);
        }

        private void MutateCardLocationDifferenceMultiplyer()
        {
            CardLocationDifferenceMultiplyer = GetRandomNumber(.001, .01);
        }

        private void MutateCardValueMultiplyer()
        {
            CardValueMultiplyer = GetRandomNumber(.005, .015);
        }

        private double GetRandomNumber(double minimum, double maximum)
        {
            return Random.NextDouble() * (maximum - minimum) + minimum;
        }

        public int Compare(Chromosome x, Chromosome y)
        {
            double xRank = x.GetChromosomeRankValue();
            double yRank = y.GetChromosomeRankValue();
            double xAverage = x.GameLegthInTurnsAverage();
            double yAverage = y.GameLegthInTurnsAverage();
            // Sort by win percentage first
            if (xRank < yRank)
            {
                return 1;
            }
            else if (xRank > yRank)
            {
                return -1;
            }
            // Then sort by number of survived generations if needed
            else if (x.NumberOfGenerationsSurvived < y.NumberOfGenerationsSurvived)
            {
                return 1;
            }
            else if (x.NumberOfGenerationsSurvived > y.NumberOfGenerationsSurvived)
            {
                return -1;
            }
            // Then sort by game length if needed
            else if (xAverage > yAverage)
            {
                return 1;
            }
            else if (xAverage < yAverage)
            {
                return -1;
            }
            // Otherwise it is equal
            else
            {
                return 0;
            }
        }
    }
}
