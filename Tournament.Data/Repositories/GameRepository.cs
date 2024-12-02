using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly TournamentApiContext _context;

        public GameRepository(TournamentApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game> GetAsync(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                throw new KeyNotFoundException($"Game with ID {id} not found.");
            }

            return game;
        }


        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Games.AnyAsync(g => g.Id == id);
        }

        public void Add(Game game)
        {
            _context.Games.Add(game);
        }

        public void Update(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
        }

        public void Remove(Game game)
        {
            _context.Games.Remove(game);
        }
    }
}
