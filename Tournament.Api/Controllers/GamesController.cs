using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Tournament.Core.Entities;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly TournamentApiContext _context;

        public GamesController(TournamentApiContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            // Include related TournamentDetails in the response
            return await _context.Games.Include(g => g.TournamentDetails).ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game?>> GetGame(int id)
        {
            var game = await _context.Games
                                      .Include(g => g.TournamentDetails) // Include related TournamentDetails
                                      .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound($"Game with ID {id} not found.");
            }

            return game;
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest("The provided game ID does not match the game entity.");
            }

            // Attach the entity and set its state to Modified
            _context.Attach(game);
            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound($"Game with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }

            // Return the updated entity with TournamentDetails included
            return Ok(await _context.Games.Include(g => g.TournamentDetails)
                                          .FirstOrDefaultAsync(g => g.Id == id));
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            // Include TournamentDetails in the response
            var createdGame = await _context.Games.Include(g => g.TournamentDetails)
                                                  .FirstOrDefaultAsync(g => g.Id == game.Id);

            return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound($"Game with ID {id} not found.");
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
