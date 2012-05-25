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
    public class INotifyCollectionChangedWeakEventManager
        : WeakEventManagerBase<
        INotifyCollectionChanged,
        INotifyCollectionChangedWeakEventManager>
    {

        protected override void StartListening(object source)
        {
            var castedSource = source as INotifyCollectionChanged;
            if (castedSource == null) return;

            castedSource.CollectionChanged += this.DeliverEvent;
        }

        protected override void StopListening(object source)
        {
            var castedSource = source as INotifyCollectionChanged;
            if (castedSource == null) return;

            castedSource.CollectionChanged -= this.DeliverEvent;
        }



    }
}
