using AutoMapper;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class GameService : IGameService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GameDto> CreateAsync(GameDto gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);

            _unitOfWork.GameRepository.Add(game);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<GameDto>(game);
        }

        public async Task DeleteAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null) throw new ArgumentException("Game not found");

            _unitOfWork.GameRepository.Remove(game);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<(IEnumerable<GameDto>, PaginationMetadata)> GetAllAsync(int page, int pageSize)
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            var totalItems = games.Count();

            pageSize = Math.Clamp(pageSize, 1, 100);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = Math.Clamp(page, 1, totalPages);

            var pagedGames = games.Skip((page - 1) * pageSize).Take(pageSize);
            var GamesDtos = _mapper.Map<IEnumerable<GameDto>>(pagedGames);

            var metadata = new PaginationMetadata(totalPages, pageSize, page, totalItems);

            return (GamesDtos, metadata);
        }

        public async Task<GameDto?> GetByIdAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            return _mapper.Map<GameDto>(game);
        }

        public async Task UpdateAsync(int id, GameDto gameDto)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null) throw new ArgumentException("Game not found");

            _mapper.Map(gameDto, game);
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.CompleteAsync();
        }
    }
}
