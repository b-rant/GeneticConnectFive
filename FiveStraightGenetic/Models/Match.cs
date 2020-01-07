using FiveStraightGame;
using System;

namespace FiveStraightGenetic.Models
{
    public class Match
    {
        public Match(Chromosome player0, Chromosome player1)
        {
            Game = new Game();
            Player0 = player0;
            Player1 = player1;
        }

        public Chromosome Player0 { get; }

        public Chromosome Player1 { get; }

        public int GameLengthInTurns { get; private set; }

        public Guid WinningPlayer { get; private set; }

        public Game Game { get;}

        /// <summary>
        /// Sets the GameLengthInTurns and Determines Winning Players GUID Id
        /// </summary>
        public void DetermineWinner()
        {
            GameLengthInTurns = Game.TurnNumber;

            if (Game.WinningPlayer.PlayerNumber == 0)
            {
                WinningPlayer = Player0.Id;
                return;
            }
            WinningPlayer = Player1.Id;
            return;
        }
    }
}
