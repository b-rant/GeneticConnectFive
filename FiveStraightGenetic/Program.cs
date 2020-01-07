using FiveStraightGame;
using FiveStraightGenetic.Models;
using FiveStraightGenetic.Utility;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FiveStraightGenetic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Lets Play Some 5ive Straight!!");

            var player0Chromosome = new Chromosome()
            {
                PotentialFiveMultiplyer = 6.5,
                OffensiveMultiplyer = .8,
                DefensiveMultiplyer = .8,
                DrawMultiplyer = 25,
                CardLocationDifferenceMultiplyer = .002,
                CardValueMultiplyer = .01
            };

            var player1Chromosome = new Chromosome()
            {
                PotentialFiveMultiplyer = 6.6942361843279734,
                OffensiveMultiplyer = 0.29375629718124696,
                DefensiveMultiplyer = 0.14618343950537194,
                DrawMultiplyer = 19.241699209083663,
                CardLocationDifferenceMultiplyer = 0.0018789321965905523,
                CardValueMultiplyer = 0.0055330016140514061
            };

            PlayGame(player0Chromosome, player1Chromosome);
            Console.WriteLine("Done Playing!!");
        }


        public static void PlayGame(Chromosome chromo0, Chromosome chromo1)
        {
            var Game = new Game();
            var players = new List<AiPlayer>()
            {
                new AiPlayer(chromo0, Game.GetPlayerByNumber(0), Game),
                new AiPlayer(chromo1, Game.GetPlayerByNumber(1), Game)
            };

            DisplayFunctions.DisplayBoard(Game.Board);

            int i = 0;
            while (!Game.Won)
            {
                var playerNum = i%2;
                Console.WriteLine($"Turn: {Game.TurnNumber} --------------------------------");
                DisplayFunctions.DisplayUserHand(Game.GetPlayerByNumber(playerNum));
                var play = players[playerNum].DeterminePlay();
                play.PlayerNumber = playerNum;
                bool playResult = Game.MakePlay(play);
                if (playResult == false)
                {
                    Console.WriteLine("FAILED TO MAKE PLAY!!!");
                    break;
                }
                DisplayFunctions.DisplayPlay(Game.Plays[Game.TurnNumber - 1]);
                DisplayFunctions.DisplayBoard(Game.Board);

                i++;
            }

            DisplayFunctions.DisplayWinner(Game.WinningPlayer, Game.TurnNumber);
        } 
    }
}
