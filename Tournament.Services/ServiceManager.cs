using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Services
{
    public class ServiceManager : IServiceManager
    {
        public ITournamentService TournamentService { get; }
        public IGameService GameService { get; }

        public ServiceManager(ITournamentService tournamentService, IGameService gameService)
        {
            TournamentService = tournamentService;
            GameService = gameService;
        }
    }
}
