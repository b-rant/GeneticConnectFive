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

        public int GameLengthInTurns { get; set; }

        public Guid WinningPlayer { get; set; }

        public Game Game { get;}
    }
}
