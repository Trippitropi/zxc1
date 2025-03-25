using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Interfaces;

namespace zxc1.Monopoly
{
    public class MonopolyToken
    {
        public string Name { get; }
        public int Position { get; private set; }
        public IPlayer Owner { get; }

        public MonopolyToken(string name, IPlayer owner)
        {
            Name = name;
            Position = 0;
            Owner = owner;
        }

        public void Move(int steps)
        {
            Position = (Position + steps) % 40; // 40 клітинок на стандартному полі
        }

        public void SetPosition(int position)
        {
            Position = position % 40;
        }
    }
}
