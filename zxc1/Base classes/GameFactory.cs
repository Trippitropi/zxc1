using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Game_implementations;
using zxc1.Interfaces;
using zxc1.Monopoly;
using zxc1.Player_implementation;

namespace zxc1.Base_classes
{
    public class GameFactory : IGameFactory
    {
        public IGame CreateGame(string gameType)
        {
            switch (gameType.ToLower())
            {
                case "alias":
                    return new AliasGame();
                case "mafia":
                    var roleDistributor = new RoleDistributor();
                    MafiaNightPhase nightPhaseService = new MafiaNightPhase();
                    var dayPhaseService = new MafiaDayPhaseService();
                    return new MafiaGame(roleDistributor, nightPhaseService, dayPhaseService);
                case "monopoly":
                    return new MonopolyGame();
                default:
                    throw new ArgumentException($"Непідтримуваний тип гри: {gameType}");
            }
        }
    }
}
