using System;

namespace Tournament.Core.Dto
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate => StartDate.AddMonths(3);
    }
}
