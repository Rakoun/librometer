using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Librometer.Views.Behaviors
{
    public class SelectedItemsSyncher
        : DependencyObject
    {
        private readonly static Type NotifyCollectionChangedWeakEventMngrType
            = typeof(INotifyCollectionChangedWeakEventManager);
        private readonly static Type SelectionChangedWeakEventMngrType
           = typeof(SelectionChangedWeakEventManager);


        private readonly static List<ItemsSyncher> _cacheDeSynchers
            = new List<ItemsSyncher>();
        private static readonly object _lock = new object();

        private readonly static DispatcherTimer timerDeNettoyage;
        static SelectedItemsSyncher()
        {
            //Creation d'un time de nettoyage du cache
            timerDeNettoyage = new DispatcherTimer(DispatcherPriority.Background);
            timerDeNettoyage.Tick += (a, b) =>
            {
                lock (_lock)
                {
                    _cacheDeSynchers.RemoveAll(itemSync => !itemSync.ASynchroniserWeakRef.IsAlive);
                }

            };
            timerDeNettoyage.Interval = TimeSpan.FromMinutes(5.0);
            timerDeNettoyage.Start();
        }

        #region Source

        /// <summary>
        /// Source Read-Only Dependency Property
        /// </summary>
        private static readonly DependencyProperty SourceProperty
            = DependencyProperty.RegisterAttached("Source",
            typeof(IList), typeof(SelectedItemsSyncher),
            new PropertyMetadata((IList)null, OnSourceChanged));


        /// <summary>
        /// Gets the Source property. This dependency property 
        /// indicates ....
        /// </summary>
        public static IList GetSource(DependencyObject d)
        {
            return (IList)d.GetValue(SourceProperty);
        }

        /// <summary>
        /// Provides a secure method for setting the Source property.  
        /// This dependency property indicates ....
        /// </summary>
        public static void SetSource(DependencyObject d, IList value)
        {
            d.SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Handles changes to the Source property.
        /// </summary>
        private static void OnSourceChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            IList oldSource = (IList)e.OldValue;
            IList newSource = (IList)d.GetValue(SourceProperty);

            ListBox listBox = d as ListBox;

            if (listBox == null)
                throw new ArgumentOutOfRangeException(
                    "d",
                    "Impossible d'attacher ce Behavior sur autre chose qu'un objet de type ListBox."
                    );

            ItemsSyncher itemSyncher = ObtenirItemSyncherCorrespondant(d);

            //Créer un Syncher car il n'existe pas.
            if (itemSyncher == null)
            {
                lock (_lock)
                {
                    itemSyncher = ObtenirItemSyncherCorrespondant(d);
                    if (itemSyncher == null)
                    {
                        itemSyncher = new ItemsSyncher(listBox);
                        _cacheDeSynchers.Add(itemSyncher);
                        SelectionChangedWeakEventManager.AddListener(listBox, itemSyncher);
                    }
                }
            }


            if (oldSource is INotifyCollectionChanged)
            {
                INotifyCollectionChangedWeakEventManager
                    .RemoveListener(oldSource as INotifyCollectionChanged, itemSyncher);
            }

            if (newSource != null && newSource is INotifyCollectionChanged)
            {
                INotifyCollectionChangedWeakEventManager
                    .AddListener(newSource as INotifyCollectionChanged, itemSyncher);
            }
            else
            {
                lock (_lock)
                {
                    _cacheDeSynchers.Remove(itemSyncher);
                }
            }


        }

        #endregion


        private static ItemsSyncher ObtenirItemSyncherCorrespondant(DependencyObject d)
        {
            return _cacheDeSynchers.FirstOrDefault(
                it => it.ASynchroniserWeakRef.IsAlive && it.ASynchroniserWeakRef.Target == d
                );
        }
        private class ItemsSyncher : IWeakEventListener
        {
            public ItemsSyncher(ListBox aSynchroniser)
            {
                if (!(aSynchroniser is ListBox))
                    throw new ArgumentOutOfRangeException(
                        "aSynchroniser",
                        "Impossible de synchroniser un element de ce type.");

                aSynchroniserWeakRef = new WeakReference(aSynchroniser);
            }

            private readonly WeakReference aSynchroniserWeakRef;

            public WeakReference ASynchroniserWeakRef
            {
                get { return aSynchroniserWeakRef; }
            }

            private void SynchroniseLaListBoxAvecLaSource(object sender, EventArgs e)
            {
                var realArg = e as NotifyCollectionChangedEventArgs;
                if (realArg == null) return;

                if (!aSynchroniserWeakRef.IsAlive) return;
                ListBox listBox = aSynchroniserWeakRef.Target as ListBox;
                if (listBox == null) return; //possible

                //Toutes les collections ne mettent pas les items supprimés dans
                //e.OldItems lorsqu'un Clear est fait.
                if (realArg.Action == NotifyCollectionChangedAction.Reset)
                {
                    listBox.SelectedItems.Clear();
                }

                if (realArg.NewItems != null)
                {
                    foreach (var item in realArg.NewItems)
                    {
                        listBox.SelectedItems.Add(item);
                    }
                }

                if (realArg.OldItems != null)
                {
                    foreach (var item in realArg.OldItems)
                    {
                        listBox.SelectedItems.Remove(item);
                    }
                }

            }

            private static void SynchroniseLaSourceAvecLaListBox(object sender, EventArgs e)
            {
                IList source = SelectedItemsSyncher.GetSource(sender as DependencyObject);
                if (source == null) return;

                var realArg = e as SelectionChangedEventArgs;
                if (realArg == null) return;

                foreach (var item in realArg.AddedItems)
                {
                    source.Add(item);
                }

                foreach (var item in realArg.RemovedItems)
                {
                    source.Remove(item);
                }

            }


            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
            {
                //Evenement provenant de la collection source
                if (NotifyCollectionChangedWeakEventMngrType == managerType)
                {
                    SynchroniseLaListBoxAvecLaSource(sender, e);
                    return true;
                }

                //Evenement provenant de la listbox
                if (SelectionChangedWeakEventMngrType == managerType)
                {
                    SynchroniseLaSourceAvecLaListBox(sender, e);
                    return true;
                }


                return false; ;
            }


        }
    }
}
