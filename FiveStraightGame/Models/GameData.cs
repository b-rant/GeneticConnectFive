using Newtonsoft.Json;
using System.Collections.Generic;

namespace FiveStraightGame.Models
{
    public class GameData
    {
        public GameData()
        {
            Board = BoardFactory.BuildNewBoard();
            Deck = BoardFactory.BuildDeck();
            Players = BoardFactory.BuildPlayers();
            Plays = new List<Play>();
            TurnNumber = 1;
            Won = false;
            HighestPlayable = 99;
        }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("won")]
        public bool Won { get; set; }

        [JsonProperty("turnNumber")]
        public int TurnNumber { get; set; }

        [JsonProperty("highestPlayable")]
        public int HighestPlayable { get; set; }

        [JsonProperty("board")]
        public List<BoardLocation> Board { get; set; }

        [JsonProperty("deck")]
        public List<int> Deck { get; set; }

        [JsonProperty("plays")]
        public List<Play> Plays { get; set; }

        [JsonProperty("players")]
        public List<Player> Players { get; set; }

    }
}
