using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Base_classes;

namespace zxc1.observerPattern
{
    public class Unsubscriber : IDisposable
    {
        private List<IObserver<GameRulesEventArgs>> _observers;
        private IObserver<GameRulesEventArgs> _observer;

        public Unsubscriber(
            List<IObserver<GameRulesEventArgs>> observers,
            IObserver<GameRulesEventArgs> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}

