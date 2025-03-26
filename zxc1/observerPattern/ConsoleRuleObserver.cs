using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Base_classes;

namespace zxc1.observerPattern
{
    public class ConsoleRuleObserver : IObserver<GameRulesEventArgs>
    {
        public void OnCompleted() { }

        public void OnError(Exception error)
        {
            Console.WriteLine($"Помилка при отриманні правил: {error.Message}");
        }

        public void OnNext(GameRulesEventArgs value)
        {
            Console.WriteLine($"=== ПРАВИЛА ГРИ {value.GameName.ToUpper()} ===");
            Console.WriteLine(value.Rules);
            Console.ReadLine();
        }
    }

}
