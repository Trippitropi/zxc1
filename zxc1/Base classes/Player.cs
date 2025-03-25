using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Interfaces;

namespace zxc1.Base_classes
{
    public class Player : IPlayer
    {
        public string Name { get; }

        public Player(string name)
        { 
            Name = name;
        }
    }
}
