using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zxc1.Interfaces
{
    public interface IGameFactory
    {
        IGame CreateGame(string gameType);
    }
}
