using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Service.Contracts
{
    public interface ITournamentService
    {
        Task<(IEnumerable<TournamentDto>, PaginationMetadata)> GetAllAsync(int page, int pageSize);
        Task<TournamentDto?> GetByIdAsync(int id);
        Task<TournamentDto> CreateAsync(TournamentDto tournamentDto);
        Task UpdateAsync(int id, TournamentDto tournamentDto);
        Task DeleteAsync(int id);
    }
}
