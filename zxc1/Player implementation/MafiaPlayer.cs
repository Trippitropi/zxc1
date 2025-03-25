using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Interfaces;

namespace zxc1.Player_implementation
{
    public class MafiaPlayer : IPlayer
    {
        public string Name { get; }
        public Role Role { get; }
        public bool IsAlive { get; private set; }

        public MafiaPlayer(string name, Role role)
        {
            Name = name;
            Role = role;
            IsAlive = true;
        }

        public void Kill()
        {
            IsAlive = false;
        }

        public void Revive()
        {
            IsAlive = true;
        }
    }
}
