using System;
using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.Dto
{
    public class GameDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)] // Limit title to a maximum of 100 characters
        public string Title { get; set; } = string.Empty;
        public DateTime Time { get; set; }
    }
}
