using FiveStraightGenetic.Utility;
using System;

namespace FiveStraightGenetic.Models
{
    public class Chromosome
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
            Wins = 0;
            SumOfGameLengthsInTurns = 0;
            NumberOfGamesPlayed = 0;
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
            Wins = 0;
            SumOfGameLengthsInTurns = 0;
            NumberOfGamesPlayed = 0;
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
            Wins = 0;
            SumOfGameLengthsInTurns = 0;
            NumberOfGamesPlayed = 0;
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
        public void RandomMutate()
        {
            // Possible to mutate between 1 and 5 different properties
            int mutations = Random.Next(Configuration._MinimumNumberOfMutations, Configuration._MaxNumberOfMutations);
            for (int i = 0; i < mutations; i++)
            {
                int propToMutate = Random.Next(1, 7);

                switch (propToMutate)
                {
                    case 1:
                        MutatePotentialFiveMultiplyer();
                        break;
                    case 2:
                        MutateOffensiveMultiplyer();
                        break;
                    case 3:
                        MutateDefensiveMultiplyer();
                        break;
                    case 4:
                        MutateDrawMultiplyer();
                        break;
                    case 5:
                        MutateCardLocationDifferenceMultiplyer();
                        break;
                    case 6:
                        MutateCardValueMultiplyer();
                        break;
                    default:
                        break;
                }
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
            return SumOfGameLengthsInTurns / NumberOfGamesPlayed;
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
            return Wins / NumberOfGamesPlayed;
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

    }
}
