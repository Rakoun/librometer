using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Librometer.Views.Behaviors
{
    public abstract class WeakEventManagerBase<TAimed, TFinal> : WeakEventManager
        where TFinal : WeakEventManagerBase<TAimed, TFinal>, new()
    {


        public static void AddListener(TAimed source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }
        public static void RemoveListener(TAimed source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }


        private readonly static object _lock = new object();

        private static TFinal CreateCurrentManager()
        {
            TFinal manager;
            lock (_lock)
            {
                manager = WeakEventManager.GetCurrentManager(typeof(TFinal)) as TFinal;
                if (manager == null)
                {
                    manager = new TFinal();
                    WeakEventManager
                        .SetCurrentManager(
                        typeof(INotifyCollectionChangedWeakEventManager),
                        manager);
                }
            }
            return manager;
        }

        public static TFinal CurrentManager
        {
            get
            {
                var manager = WeakEventManager.GetCurrentManager(typeof(TFinal)) as TFinal;
                if (manager == null)
                {
                    manager = CreateCurrentManager();
                }
                return manager;
            }

        }

    }
}
