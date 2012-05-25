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
    public static class ListBoxFix
    {
        public static bool GetSelectedItemsBinding(System.Windows.Controls.ListBox element)
        {
            return (bool)element.GetValue(SelectedItemsBindingProperty);
        }

        public static void SetSelectedItemsBinding(System.Windows.Controls.ListBox element, bool value)
        {
            element.SetValue(SelectedItemsBindingProperty, value);
            if (value)
            {
                element.SelectionChanged += (sender, args) =>
                {
                    // Dummy code to refresh SelectedItems value
                    var x = element.SelectedItems;
                };
            }
        }

        public static readonly DependencyProperty SelectedItemsBindingProperty =
            DependencyProperty.RegisterAttached("FixSlecetedItemsBinding",
            typeof(bool), typeof(FrameworkElement), new PropertyMetadata(false));
    }
}
