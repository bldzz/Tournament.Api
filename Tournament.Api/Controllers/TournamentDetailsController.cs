using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Dto;
using Tournament.Core.Repositories;
using AutoMapper;
using Tournament.Core.Entities;
using Newtonsoft.Json;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentDetailsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails()
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();
            if (tournaments == null)
            {
                return NotFound("No tournaments found");
            }

            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(tournamentDtos);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound("Tournament not found");
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournamentDetails);
            return Ok(tournamentDto);
        }

        // PUT: api/TournamentDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDto tournamentDto)
        {
            if (id != tournamentDto.Id)
            {
                return BadRequest("The provided tournament ID does not match the entity.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Validation failed");
            }

            var tournamentDetails = _mapper.Map<TournamentDetails>(tournamentDto);
            _unitOfWork.TournamentRepository.Update(tournamentDetails);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
                {
                    return NotFound("Tournament not found");
                }
                else
                {
                    return StatusCode(500, "Error saving to the database.");
                }
            }

            return Ok(tournamentDto);
        }

        // PATCH: api/TournamentDetails/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTournamentDetails(int id, [FromBody] JsonPatchDocument<TournamentDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound("Tournament not found");
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournamentDetails);
            patchDocument.ApplyTo(tournamentDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(tournamentDto, tournamentDetails);
            _unitOfWork.TournamentRepository.Update(tournamentDetails);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
                {
                    return NotFound("Tournament not found");
                }
                else
                {
                    return StatusCode(500, "Error saving to the database.");
                }
            }

            return Ok(tournamentDto);
        }

        // POST: api/TournamentDetails
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentDto tournamentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Validation failed");
            }

            var tournamentDetails = _mapper.Map<TournamentDetails>(tournamentDto);
            _unitOfWork.TournamentRepository.Add(tournamentDetails);
            await _unitOfWork.CompleteAsync();

            tournamentDto.Id = tournamentDetails.Id;
            return CreatedAtAction("GetTournamentDetails", new { id = tournamentDetails.Id }, tournamentDto);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound("Tournament not found");
            }

            _unitOfWork.TournamentRepository.Remove(tournamentDetails);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }



    }
}
