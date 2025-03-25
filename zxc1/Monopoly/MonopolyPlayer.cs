using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Interfaces;

namespace zxc1.Monopoly
{
    public class MonopolyPlayer : IPlayer
    {
        public string Name { get; }
        public int Money { get; private set; }
        public MonopolyToken Token { get; }
        public bool IsBankrupt { get; private set; }
        public List<MonopolyProperty> Properties { get; }

        public MonopolyPlayer(string name, string tokenName)
        {
            Name = name;
            Money = 1500;
            Token = new MonopolyToken(tokenName, this);
            IsBankrupt = false;
            Properties = new List<MonopolyProperty>();
        }

        public void AddMoney(int amount)
        {
            Money += amount;
        }

        public void DeductMoney(int amount)
        {
            Money -= amount;
            if (Money < 0)
            {
                IsBankrupt = true;
            }
        }

        public void BuyProperty(MonopolyProperty property)
        {
            if (Money >= property.Price)
            {
                property.BuyProperty(this);
                Properties.Add(property);
            }
            else
            {
                Console.WriteLine($"У гравця {Name} недостатньо грошей для купівлі {property.Name}");
            }
        }
    }
}
