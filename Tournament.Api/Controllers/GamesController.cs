using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Dto;
using Tournament.Core.Repositories;
using AutoMapper;
using Tournament.Core.Entities;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gameDtos);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            if (id != gameDto.Id)
            {
                return BadRequest("The provided game ID does not match the game entity.");
            }

            var game = _mapper.Map<Game>(gameDto);
            _unitOfWork.GameRepository.Update(game);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.GameRepository.AnyAsync(id))
                {
                    return NotFound("Game not found");
                }
                else
                {
                    return StatusCode(500, "Error saving to the database.");
                }
            }

            return Ok(gameDto);
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Validation failed");
            }

            var game = _mapper.Map<Game>(gameDto);
            _unitOfWork.GameRepository.Add(game);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, gameDto);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound("Game not found");
            }

            _unitOfWork.GameRepository.Remove(game);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
