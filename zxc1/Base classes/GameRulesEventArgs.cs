using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zxc1.Base_classes
{
    public class GameRulesEventArgs : EventArgs
    {
        public string GameName { get; }
        public string Rules { get; }

        public GameRulesEventArgs(string gameName, string rules)
        {
            GameName = gameName;
            Rules = rules;
        }
    }
}
