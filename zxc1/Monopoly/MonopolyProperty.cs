using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zxc1.Monopoly
{
    public class MonopolyProperty
    {
        public string Name { get; }
        public int Price { get; }
        public int Rent { get; private set; }
        public MonopolyPlayer Owner { get; private set; }
        public int Position { get; }

        public MonopolyProperty(string name, int position, int price, int initialRent)
        {
            Name = name;
            Position = position;
            Price = price;
            Rent = initialRent;
            Owner = null;
        }

        public void BuyProperty(MonopolyPlayer player)
        {
            if (Owner == null && player.Money >= Price)
            {
                player.DeductMoney(Price);
                Owner = player;
                Console.WriteLine($"Гравець {player.Name} купив власність {Name} за {Price}$");
            }
        }

        public void PayRent(MonopolyPlayer player)
        {
            if (Owner != null && player != Owner)
            {
                player.DeductMoney(Rent);
                Owner.AddMoney(Rent);
                Console.WriteLine($"Гравець {player.Name} сплатив оренду {Rent}$ гравцю {Owner.Name} за {Name}");
            }
        }
    }
}
