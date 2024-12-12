using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentApiContext _context;

        public TournamentRepository(TournamentApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TournamentDetails>> GetAllAsync()
        {
            return await _context.TournamentDetails.Include(t => t.Games).ToListAsync();
        }

        public async Task<TournamentDetails> GetAsync(int id)
        {
            var tournament = await _context.TournamentDetails.Include(t => t.Games)
                                                             .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
            {
                throw new KeyNotFoundException($"Tournament with ID {id} not found.");
            }

            return tournament;
        }


        public async Task<bool> AnyAsync(int id)
        {
            return await _context.TournamentDetails.AnyAsync(t => t.Id == id);
        }

        public void Add(TournamentDetails tournament)
        {
            _context.TournamentDetails.Add(tournament);
        }

        public void Update(TournamentDetails tournament)
        {
            _context.Entry(tournament).State = EntityState.Modified;
        }

        public void Remove(TournamentDetails tournament)
        {
            _context.TournamentDetails.Remove(tournament);
        }
    }
}
