using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
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
            if (games == null)
            {
                return NotFound("No games found");
            }

            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gameDtos);
        }

        // GET: api/Games/title/{title}
        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesByTitle(string title)
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            var filteredGames = games.Where(g => g.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));

            if (!filteredGames.Any())
            {
                return NotFound($"No games found with the title '{title}'");
            }

            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(filteredGames);
            return Ok(gameDtos);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            if (id != gameDto.Id)
            {
                return BadRequest("The provided game ID does not match the game entity.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Validation failed");
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

        // PATCH: api/Games/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchGame(int id, [FromBody] JsonPatchDocument<GameDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound("Game not found");
            }

            var gameDto = _mapper.Map<GameDto>(game);
            patchDocument.ApplyTo(gameDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(gameDto, game);
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

            gameDto.Id = game.Id;
            return CreatedAtAction(nameof(GetGames), new { id = game.Id }, gameDto);
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
