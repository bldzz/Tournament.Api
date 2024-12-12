using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Data  // Change this to the correct namespace of your Data project if needed
{
    public class TournamentApiContext : DbContext
    {
        public TournamentApiContext(DbContextOptions<TournamentApiContext> options)
            : base(options)
        {
        }

        public DbSet<TournamentDetails> TournamentDetails { get; set; } = default!;
        public DbSet<Game> Games { get; set; } = default!;  // Changed 'Game' to 'Games' to follow convention
    }
}
