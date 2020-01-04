using FiveStraightGame.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiveStraightGenetic.Utility
{
    public static class DisplayFunctions
    {
        private const ConsoleColor Player0Color = ConsoleColor.Red;
        private const ConsoleColor Player1Color = ConsoleColor.Green;
        private const ConsoleColor WinColor = ConsoleColor.Cyan;
        private const ConsoleColor MainColor = ConsoleColor.White;

        public static void DisplayPlay(Play play)
        {
            if (play.PlayerNumber == 0)
            {
                Console.ForegroundColor = Player0Color;
            }
            else
            {
                Console.ForegroundColor = Player1Color;
            }

            if (play.Draw)
            {
                Console.WriteLine($"Player {play.PlayerNumber} drew card: |{play.CardNumber}|");
            }
            else
            {
                Console.WriteLine($"Player {play.PlayerNumber} played the |{play.CardNumber}| in the [{play.PlayedLocationNumber}]");
            }
            Console.ForegroundColor = MainColor;
        }

        public static void DisplayUserHand(Player player)
        {
            if (player.PlayerNumber == 0)
            {
                Console.ForegroundColor = Player0Color;
                Console.Write($"Player 0 hand: ");
            }
            else
            {
                Console.ForegroundColor = Player1Color;
                Console.Write($"Player 1 hand: ");
            }
            foreach (var card in player.Hand)
            {
                Console.Write($"|{card}| ");
            }
            Console.Write("\n");
            Console.ForegroundColor = MainColor;
        }

        public static void DisplayWinner(Player player, int turnNumber)
        {
            Console.ForegroundColor = WinColor;
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Player {player.PlayerNumber} has Won in {turnNumber} Turns!!!!!");
            Console.WriteLine("----------------------------------------");
            Console.ForegroundColor = MainColor;
        }

        public static void DisplayBoard(List<BoardLocation> board)
        {
            Console.WriteLine("-----------------Board------------------");
            DisplayRow(board, board[73]);
            Console.Write("\n");
            DisplayRow(board, board[74]);
            Console.Write("\n");
            DisplayRow(board, board[75]);
            Console.Write("\n");
            DisplayRow(board, board[76]);
            Console.Write("\n");
            DisplayRow(board, board[77]);
            Console.Write("\n");
            DisplayRow(board, board[78]);
            Console.Write("\n");
            DisplayRow(board, board[79]);
            Console.Write("\n");
            DisplayRow(board, board[80]);
            Console.Write("\n");
            DisplayRow(board, board[81]);
            Console.Write("\n");
            DisplayRow(board, board[82]);
            Console.Write("\n");
            Console.WriteLine("-----------------Board------------------");
        }

        private static void DisplayRow(List<BoardLocation> board, BoardLocation loc)
        {
            int? nextLoc = loc.Number;
            for (int i = 0; i < 10; i++)
            {
                WriteLocString(loc);
                nextLoc = loc.AdjacentLocations[3];
                if (nextLoc == null)
                {
                    break;
                }
                loc = board[(int)nextLoc];
            }
        }

        private static void WriteLocString(BoardLocation location)
        {
            switch (location.FilledBy)
            {
                case -1:
                    Console.Write($"[{location.DisplayNumber}]");
                    break;
                case 0:
                    Console.ForegroundColor = Player0Color;
                    Console.Write($"[{location.DisplayNumber}]");
                    Console.ForegroundColor = MainColor;
                    break;
                case 1:
                    Console.ForegroundColor = Player1Color;
                    Console.Write($"[{location.DisplayNumber}]");
                    Console.ForegroundColor = MainColor;
                    break;
                default:
                    Console.Write("ERROR");
                    break;
            }
        }

    }
}
