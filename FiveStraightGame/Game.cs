using FiveStraightGame.Models;
using System.Collections.Generic;
using System.Linq;

namespace FiveStraightGame
{
    public class Game
    {
        public Game()
        {
            Board = BoardFactory.BuildNewBoard();
            Deck = BoardFactory.BuildDeck();
            Players = BoardFactory.BuildPlayers();
            Plays = new List<Play>();
            TurnNumber = 0;
            Won = false;
            HighestPlayable = 99;
            CurrentPlayer = Players[0];
            Deal();
        }

        public string Id { get; internal set; }
        public bool Won { get; internal set; }
        public int TurnNumber { get; internal set; }
        public int HighestPlayable { get; internal set; }
        public List<BoardLocation> Board { get; internal set; }
        public List<int> Deck { get; internal set; }
        public List<Play> Plays { get; internal set; }
        private List<Player> Players { get; set; }
        private Player CurrentPlayer { get; set; }
        public Player WinningPlayer { get; internal set; }

        // Public Functions

        public Player GetPlayerByNumber(int playerNumber)
        {
            return Players[playerNumber];
        }

        public bool MakePlay(Play play)
        {
            if (play.Draw)
            {
                return PlayDrawCard(Players[play.PlayerNumber]);
            }
            else
            {
                return PlayLocation(Players[play.PlayerNumber], play.PlayedLocationNumber, play.CardNumber);
            }
        }

        public bool PlayLocation(Player player, int location, int card)
        {
            if (!CanPlay(location, card, player))
            {
                return false;
            }

            RemoveCardFromHand(player, card);
            FillLocation(player, location);

            if (location.Equals(HighestPlayable))
            {
                DetermineHighestPlayable();
            }

            Plays.Add(new Play()
            {
                CardNumber = card,
                PlayedLocationNumber = location,
                Draw = false,
                PlayerHand = player.Hand,
                PlayerNumber = player.PlayerNumber,
                TurnNumber = TurnNumber
            });

            if (CheckWinCondition(location, player))
            {
                Won = true;
                WinningPlayer = CurrentPlayer;
            }
            else
            {
                CheckForAllDeadCards();
            }

            NextTurn();
            return true;
        }

        public bool PlayDrawCard(Player player)
        {
            if (!CanDraw(player))
            {
                return false;
            }

            var cardDrew = DrawCard();
            AddCardToHand(player, cardDrew);

            Plays.Add(new Play()
            {
                CardNumber = cardDrew,
                PlayedLocationNumber = -1,
                Draw = true,
                PlayerHand = player.Hand,
                PlayerNumber = player.PlayerNumber,
                TurnNumber = TurnNumber
            });

            CheckForAllDeadCards();

            NextTurn();
            return true;
        }


        // Private Functions

        private void Deal()
        {
            // deals 4 cards round robin
            for (int i = 0; i < 4; i++)
            {
                foreach (var player in Players)
                {
                    player.Hand.Add(DrawCard());
                }
            }
        }

        private int DrawCard()
        {
            var card = Deck[0];
            Deck.RemoveAt(0);
            return card;
        }

        private void AddCardToHand(Player player, int card)
        {
            player.Hand.Add(card);
        }

        private void RemoveCardFromHand(Player player, int card)
        {
            player.Hand.Remove(card);
        }

        private void NextTurn()
        {
            TurnNumber++;
            CurrentPlayer = Players[TurnNumber % Players.Count];
        }

        private void FillLocation(Player player, int locationNumber)
        {
            Board[locationNumber].Filled = true;
            Board[locationNumber].FilledBy = player.PlayerNumber;
        }

        private bool CheckWinCondition(int location, Player player)
        {
            var NW_SE = RecursiveBoardWinSearch(location, player.PlayerNumber, 0) + RecursiveBoardWinSearch(location, player.PlayerNumber, 4);
            var N_S = RecursiveBoardWinSearch(location, player.PlayerNumber, 1) + RecursiveBoardWinSearch(location, player.PlayerNumber, 5);
            var NE_SW = RecursiveBoardWinSearch(location, player.PlayerNumber, 2) + RecursiveBoardWinSearch(location, player.PlayerNumber, 6);
            var W_E = RecursiveBoardWinSearch(location, player.PlayerNumber, 3) + RecursiveBoardWinSearch(location, player.PlayerNumber, 7);

            if (NW_SE >= 6 || N_S >= 6 || NE_SW >= 6 || W_E >= 6)
            {
                return true;
            }
            return false;
        }

        private int RecursiveBoardWinSearch(int location, int playerNumber, int direction)
        {
            //if the location is filled by the current user
            if (Board[location].FilledBy == playerNumber)
            {
                //if it is, then check the next locaiton.
                var tempDirection = Board[location].AdjacentLocations[direction];
                //if the next location is undefined, it is out of the board so stop looking
                if (tempDirection == null)
                {
                    return 1;
                };
                return (1 + RecursiveBoardWinSearch((int)tempDirection, playerNumber, direction));
            }
            return 0;
        }

        private bool CanPlay(int location, int card, Player player)
        {
            if (!Won
                && location >= 0 
                && location <= 99
                && location >= card 
                && !Board[location].Filled 
                && player.Hand.Contains(card)
                && CurrentPlayer.Equals(player))
            {
                return true;
            }
            return false;
        }

        private bool CanDraw(Player player)
        {
            if (!Won 
                && player.Hand.Count < 4 
                && CurrentPlayer.Equals(player))
            {
                return true;
            }
            return false;
        }

        private void DetermineHighestPlayable()
        {
            for (int i = HighestPlayable; i > 0; i--)
            {
                if (!Board[i].Filled)
                {
                    HighestPlayable = i;
                    return;
                }
            }
            HighestPlayable = 0;
            return;
        }

        private void CheckForAllDeadCards()
        {
            foreach (var player in Players)
            {
                if (player.Hand.Where(x => x > HighestPlayable).Count() == 4)
                {
                    Won = true;
                    WinningPlayer = Players[(player.PlayerNumber + 1) % 2];
                    return;
                }
            }
        }
    }
}
