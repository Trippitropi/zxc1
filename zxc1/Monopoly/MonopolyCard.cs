using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zxc1.Monopoly
{
    public class MonopolyCard
    {
        public string Description { get; }
        public CardType Type { get; }
        public Action<MonopolyPlayer> Effect { get; }

        public MonopolyCard(string description, CardType type, Action<MonopolyPlayer> effect)
        {
            Description = description;
            Type = type;
            Effect = effect;
        }
    }

}
