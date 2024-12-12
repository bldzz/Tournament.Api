using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Service.Contracts
{
    public interface IGameService
    {
        Task<(IEnumerable<GameDto>, PaginationMetadata)> GetAllAsync(int page, int pageSize);
        Task<GameDto?> GetByIdAsync(int id);
        Task<GameDto> CreateAsync(GameDto gameDto);
        Task UpdateAsync(int id, GameDto gameDto);
        Task DeleteAsync(int id);
    }
}
