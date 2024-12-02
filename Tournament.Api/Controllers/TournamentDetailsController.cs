using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Tournament.Core.Entities;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly TournamentApiContext _context;

        public TournamentDetailsController(TournamentApiContext context)
        {
            _context = context;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDetails>>> GetTournamentDetails()
        {
            return await _context.TournamentDetails
                                 .Include(t => t.Games) // Include related Games
                                 .ToListAsync();
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDetails>> GetTournamentDetails(int id)
        {
            var tournamentDetails = await _context.TournamentDetails
                                                  .Include(t => t.Games) // Include related Games
                                                  .FirstOrDefaultAsync(t => t.Id == id);

            if (tournamentDetails == null)
            {
                return NotFound();
            }

            return tournamentDetails;
        }

        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDetails tournamentDetails)
        {
            if (id != tournamentDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(tournamentDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Return the updated tournamentDetails with Games included
            return Ok(await _context.TournamentDetails
                                    .Include(t => t.Games)
                                    .FirstOrDefaultAsync(t => t.Id == id));
        }

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDetails>> PostTournamentDetails(TournamentDetails tournamentDetails)
        {
            _context.TournamentDetails.Add(tournamentDetails);
            await _context.SaveChangesAsync();

            // Include related Games in the response
            var createdTournament = await _context.TournamentDetails
                                                  .Include(t => t.Games)
                                                  .FirstOrDefaultAsync(t => t.Id == tournamentDetails.Id);

            return CreatedAtAction("GetTournamentDetails", new { id = createdTournament.Id }, createdTournament);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournamentDetails = await _context.TournamentDetails
                                                  .Include(t => t.Games) // Include related Games for proper deletion
                                                  .FirstOrDefaultAsync(t => t.Id == id);

            if (tournamentDetails == null)
            {
                return NotFound();
            }

            _context.TournamentDetails.Remove(tournamentDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TournamentDetailsExists(int id)
        {
            return _context.TournamentDetails.Any(e => e.Id == id);
        }
    }
}
