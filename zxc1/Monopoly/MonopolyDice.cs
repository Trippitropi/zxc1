using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zxc1.Monopoly
{
    public class MonopolyDice
    {
        private readonly Random _random;

        public MonopolyDice()
        {
            _random = new Random();
        }

        public int Roll()
        {
            return _random.Next(1, 7);
        }

        public (int, int) RollTwo()
        {
            return (Roll(), Roll());
        }
    }
}
