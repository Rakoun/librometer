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
using System.Collections;

namespace Librometer.Views.Behaviors
{
    public static class SelectedItemsHelper
    {
        /// <summary>
        /// SelectedItems Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =

        DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(SelectedItemsHelper),
        /*RGE new FrameworkPropertyMetadata((IList)null,*/
        new PropertyMetadata((IList)null,
        new PropertyChangedCallback(OnSelectedItemsChanged)));
        /// <summary>
        /// Gets the SelectedItems property. This dependency property
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static IList GetSelectedItems(DependencyObject d)
        {
            return (IList)d.GetValue(SelectedItemsProperty);

        }

        /// <summary>
        /// Sets the SelectedItems property. This dependency property
        /// indicates ....
        /// </summary>
        public static void SetSelectedItems(DependencyObject d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// Handles changes to the SelectedItems property.
        /// </summary>
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (ListBox)d;
            ReSetSelectedItems(listBox);
            listBox.SelectionChanged += delegate
            {
                ReSetSelectedItems(listBox);
            };
        }

        private static void ReSetSelectedItems(ListBox listBox)
        {
            IList selectedItems = GetSelectedItems(listBox);
            selectedItems.Clear();
            if (listBox.SelectedItems != null)
            {
                foreach (var item in listBox.SelectedItems)
                    selectedItems.Add(item);
            }
        }
    }

}
