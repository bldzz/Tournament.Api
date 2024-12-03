using System;
using System.Text.Json.Serialization;

namespace Tournament.Core.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Time { get; set; }

        // Foreign key to TournamentDetails
        public int TournamentId { get; set; }

        // Navigation property to TournamentDetails

        public TournamentDetails? TournamentDetails { get; set; } // Made nullable to avoid errors during POST
    }
}
