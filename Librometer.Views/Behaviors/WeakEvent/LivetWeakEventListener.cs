using System;

namespace Librometer.Views.Behaviors
{
    public class LivetWeakEventListener<THandler, TEventArgs> : IDisposable where TEventArgs : EventArgs
    {
        private bool _disposed = false;

        private EventHandler<TEventArgs> _handler;
        private THandler _resultHandler;
        private Action<THandler> _remove;

        private static void ReceiveEvent(WeakReference listenerWeakReference, object sender, TEventArgs args)
        {
            var listener = listenerWeakReference.Target as LivetWeakEventListener<THandler, TEventArgs>;

            if (listener != null)
            {
                var handler = listener._handler;

                if (handler != null)
                {
                    handler(sender, args);
                }
            }
        }

        private static THandler GetStaticHandler(WeakReference listenerWeakReference, Func<EventHandler<TEventArgs>, THandler> conversion)
        {
            return conversion((sender, e) => ReceiveEvent(listenerWeakReference, sender, e));
        }

        public LivetWeakEventListener(Func<EventHandler<TEventArgs>, THandler> conversion, Action<THandler> add, Action<THandler> remove, EventHandler<TEventArgs> handler)
        {
            if (conversion == null) throw new ArgumentNullException("conversion");
            if (add == null) throw new ArgumentNullException("add");
            if (remove == null) throw new ArgumentNullException("remove");
            if (handler == null) throw new ArgumentNullException("handler");

            _handler = handler;
            _remove = remove;

            _resultHandler = GetStaticHandler(new WeakReference(this), conversion);

            add(_resultHandler);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _remove(_resultHandler);
            }
            _disposed = true;
        }
    }
}

