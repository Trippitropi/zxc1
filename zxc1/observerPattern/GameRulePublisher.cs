using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Base_classes;

namespace zxc1.observerPattern
{
    internal class GameRulePublisher : IObservable<GameRulesEventArgs>
    {
        private List<IObserver<GameRulesEventArgs>> _observers = new List<IObserver<GameRulesEventArgs>>();


        public event EventHandler<GameRulesEventArgs> RulesAnnounced;

        public IDisposable Subscribe(IObserver<GameRulesEventArgs> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber(_observers, observer);
        }

        public void PublishRules(string gameName, string rules)
        {
            var ruleEventArgs = new GameRulesEventArgs(gameName, rules);


            RulesAnnounced?.Invoke(this, ruleEventArgs);


            foreach (var observer in _observers)
            {
                observer.OnNext(ruleEventArgs);
                observer.OnCompleted();
            }
        }
    }
}
