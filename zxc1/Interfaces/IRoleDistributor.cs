using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Player_implementation;

namespace zxc1.Interfaces
{
    public interface IRoleDistributor
    {
        Dictionary<Role, List<MafiaPlayer>> DistributeRoles(List<IPlayer> players);
    }
}
