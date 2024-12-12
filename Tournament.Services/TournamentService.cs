using AutoMapper;
using Service.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<TournamentDto>, PaginationMetadata)> GetAllAsync(int page, int pageSize)
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();
            var totalItems = tournaments.Count();

            pageSize = Math.Clamp(pageSize, 1, 100);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = Math.Clamp(page, 1, totalPages);

            var pagedTournaments = tournaments.Skip((page - 1) * pageSize).Take(pageSize);
            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(pagedTournaments);

            var metadata = new PaginationMetadata(totalPages, pageSize, page, totalItems);

            return (tournamentDtos, metadata);
        }

        public async Task<TournamentDto?> GetByIdAsync(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);
            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task<TournamentDto> CreateAsync(TournamentDto tournamentDto)
        {
            var tournament = _mapper.Map<TournamentDetails>(tournamentDto);

            _unitOfWork.TournamentRepository.Add(tournament);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task UpdateAsync(int id, TournamentDto tournamentDto)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournament == null) throw new ArgumentException("Tournament not found");

            _mapper.Map(tournamentDto, tournament);
            _unitOfWork.TournamentRepository.Update(tournament);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournament == null) throw new ArgumentException("Tournament not found");

            _unitOfWork.TournamentRepository.Remove(tournament);
            await _unitOfWork.CompleteAsync();
        }
    }
}
