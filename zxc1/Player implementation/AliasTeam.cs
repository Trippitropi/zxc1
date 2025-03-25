using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Interfaces;


namespace zxc1.players
{
    public class AliasTeam
    {
        public string Name { get; }
        public List<IPlayer> Players { get; }
        public int Points { get; private set; }


        public AliasTeam(string name, List<IPlayer> players)
        {
            Name = name;
            Players = players;
            Points = 0;
        }

        public void AddPoint()
        {
            Points++;
        }
        public void AddPoints(int points)
        {
            Points += points;
        }
    }
}
