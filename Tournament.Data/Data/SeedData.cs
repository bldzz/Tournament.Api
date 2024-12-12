using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Tournament.Data.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(TournamentApiContext context)
        {
            if (!await context.TournamentDetails.AnyAsync())
            {
                var tournaments = new List<TournamentDetails>
                {
                    new TournamentDetails
                    {
                        Title = "Summer Tournament",
                        StartDate = DateTime.Now.AddMonths(-2),
                        Games = new List<Game>
                        {
                            new Game { Title = "Match 1", Time = DateTime.Now.AddMonths(-2).AddHours(1) },
                            new Game { Title = "Match 2", Time = DateTime.Now.AddMonths(-2).AddHours(2) }
                        }
                    },
                    new TournamentDetails
                    {
                        Title = "Winter Tournament",
                        StartDate = DateTime.Now.AddMonths(-1),
                        Games = new List<Game>
                        {
                            new Game { Title = "Match 3", Time = DateTime.Now.AddMonths(-1).AddHours(1) },
                            new Game { Title = "Match 4", Time = DateTime.Now.AddMonths(-1).AddHours(2) }
                        }
                    }
                };

                await context.TournamentDetails.AddRangeAsync(tournaments);
                await context.SaveChangesAsync();
            }
        }
    }
}
