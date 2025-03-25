using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Interfaces;

namespace zxc1.Player_implementation
{
    public class RoleDistributor : IRoleDistributor
    {
        public readonly Random _random;

        public RoleDistributor()
        {
            _random = new Random();
        }
        public Dictionary<Role, List<MafiaPlayer>> DistributeRoles(List<IPlayer> players)
        {
            if (players.Count < 4)
            {
                throw new InvalidOperationException("Недостатньо гравців! Потрібно мінімум 4 гравця.");
            }
            Dictionary<Role, List<MafiaPlayer>> roles = new Dictionary<Role, List<MafiaPlayer>>
        {
            { Role.Civilian, new List<MafiaPlayer>() },
            { Role.Mafia, new List<MafiaPlayer>() },
            { Role.Commissioner, new List<MafiaPlayer>() },
            { Role.Doctor, new List<MafiaPlayer>() }
        };

            List<IPlayer> playersCopy = new List<IPlayer>(players);
            ShuffleList(playersCopy);

            int mafiaCount = Math.Max(1, playersCopy.Count / 4);

          
            foreach (IPlayer player in playersCopy)
            {
                if (mafiaCount > 0)
                {
                    roles[Role.Mafia].Add(new MafiaPlayer(player.Name, Role.Mafia));
                    mafiaCount--;
                }
                else if (roles[Role.Commissioner].Count == 0)
                {
                    roles[Role.Commissioner].Add(new MafiaPlayer(player.Name, Role.Commissioner));
                }
                else if (roles[Role.Doctor].Count == 0)
                {
                    roles[Role.Doctor].Add(new MafiaPlayer(player.Name, Role.Doctor));
                }
                else
                {
                    roles[Role.Civilian].Add(new MafiaPlayer(player.Name, Role.Civilian));
                }
            }

            return roles;
        }
        private void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}
