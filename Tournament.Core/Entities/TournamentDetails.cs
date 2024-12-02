using System;
using System.Collections.Generic;

namespace Tournament.Core.Entities
{
    public class TournamentDetails
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }

        // Initialize the Games collection to avoid null reference issues
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
