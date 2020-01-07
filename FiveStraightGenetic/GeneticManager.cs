using FiveStraightGenetic.Models;
using FiveStraightGenetic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveStraightGenetic
{
    public class GeneticManager
    {

        public List<Chromosome> RankedInputGeneration;

        // Return list of all updated chromosomes with their game results included
        
        public List<Chromosome> ProcessGeneration(List<Chromosome> inputGeneration)
        {
            // Generate list of matches for the generation
            List<Match> matches = GenerateMatches(inputGeneration);

            // Play all the matches
            // Might swap this with a parallel foreach which is why it is not combined with the loop below
            //foreach (var match in matches)
            //{
            //    PlayGame(match);
            //}
            Parallel.ForEach(matches, match =>
            {
                PlayGame(match);
            });

            // Process all match results to update chromosome match stats
            foreach (var match in matches)
            {
                match.Player0.ProcessMatch(match);
                match.Player1.ProcessMatch(match);
            }

            // Rank the generation based on their match results
            RankedInputGeneration = inputGeneration.OrderBy(item => item, new Chromosome()).ToList();

            // Return a new generation
            return GenerateNextGeneration();

        }

        private List<Match> GenerateMatches(List<Chromosome> generation)
        {
            List<Match> matches = new List<Match>();
            int chromosomeIndex = 1;
            foreach (var chromosome in generation)
            {
                for (int i = chromosomeIndex; i < generation.Count; i++)
                {
                    matches.Add(new Match(chromosome, generation[i]));
                }
                chromosomeIndex++;
            }

            return matches;
        }

        // This function ideally should be thread safe. The only changes being made are to the unique properties in each match
        // The chromosomes in each match are ONLY being read from
        private void PlayGame(Match match)
        {
            var players = new List<AiPlayer>()
            {
                new AiPlayer(match.Player0, match.Game.GetPlayerByNumber(0), match.Game),
                new AiPlayer(match.Player1, match.Game.GetPlayerByNumber(1), match.Game)
            };

            int turnNumber = 0;
            while (!match.Game.Won)
            {
                var playerNum = turnNumber % 2;
                var play = players[playerNum].DeterminePlay();
                play.PlayerNumber = playerNum;
                bool playResult = match.Game.MakePlay(play);
                if (playResult == false)
                {
                    break;
                }
                turnNumber++;

                // Just to make sure something does not break and get hung
                if (turnNumber > 500)
                {
                    throw new ApplicationException("Game is taking way too long to run!!");
                }
            }

            match.DetermineWinner();
        }

        // This is the main Genetic algorithm configuration function. 
        private List<Chromosome> GenerateNextGeneration()
        {
            List<Chromosome> nextGeneration = new List<Chromosome>();

            // Copy the top x to the new generation
            for (int i = 0; i < Configuration._NumOfTopChromosomesToCopyOver; i++)
            {
                nextGeneration.Add(new Chromosome(RankedInputGeneration[i]));
            }

            // Crossover first place
            int index = 1;
            for (int i = 0; i < Configuration._NumOfCrossOverForFirstPlace; i++)
            {
                nextGeneration.Add(new Chromosome(RankedInputGeneration[0], RankedInputGeneration[index]));
                index++;
            }
            // Crossover second place
            index = 2;
            for (int i = 0; i < Configuration._NumOfCrossOverForSecondPlace; i++)
            {
                nextGeneration.Add(new Chromosome(RankedInputGeneration[0], RankedInputGeneration[index]));
                index++;
            }
            // Crossover third place
            index = 3;
            for (int i = 0; i < Configuration._NumOfCrossOverForThirdPlace; i++)
            {
                nextGeneration.Add(new Chromosome(RankedInputGeneration[0], RankedInputGeneration[index]));
                index++;
            }
            // Crossover fourth place
            index = 4;
            for (int i = 0; i < Configuration._NumOfCrossOverForFourthPlace; i++)
            {
                nextGeneration.Add(new Chromosome(RankedInputGeneration[0], RankedInputGeneration[index]));
                index++;
            }
            // Crossover fifth place
            index = 5;
            for (int i = 0; i < Configuration._NumOfCrossOverForFifthPlace; i++)
            {
                nextGeneration.Add(new Chromosome(RankedInputGeneration[0], RankedInputGeneration[index]));
                index++;
            }

            // Add some random new Chromosomes
            for (int i = 0; i < Configuration._NumOfRandomNewChromosomes; i++)
            {
                nextGeneration.Add(new Chromosome());
            }

            // Run mutation, but dont touch the top Copied Chromosomes
            // This generally wont mutate all of them as the mutation chance is low
            for (int i = Configuration._NumOfTopChromosomesToCopyOver; i < nextGeneration.Count; i++)
            {
                nextGeneration[i].Mutate();
            }

            return nextGeneration;
        }
    }
}
