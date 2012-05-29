using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Phone.Shell;

namespace WPExtensions
{
   
    public partial class Extensions
    {
        #region UpdateBindingOnChangeProperty

        public static DependencyProperty UpdateBindingOnChangeProperty = 
            DependencyProperty.RegisterAttached("UpdateBindingOnChange", 
            typeof (bool), typeof (Extensions), 
            new PropertyMetadata(default(bool)));

        public static bool GetUpdateBindingOnChange(FrameworkElement element)
        {
            return (bool) element.GetValue(UpdateBindingOnChangeProperty);
        }

        public static void SetUpdateBindingOnChange(FrameworkElement element, bool value)
        {
            Action<TextBox, bool> action = (txtBox, updateValue) =>
            {
                if (GetUpdateBindingOnChange(txtBox) == updateValue) return;


                if (updateValue)
                {
                    txtBox.TextChanged += element_TextChanged;
                }
                else
                {
                    txtBox.TextChanged -= element_TextChanged;
                }


                txtBox.SetValue(UpdateBindingOnChangeProperty, updateValue);
            };

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (element is TextBox)
                {
                    action.Invoke(element as TextBox, value);
                    return;
                }

                element.GetChildrens<TextBox>().ForEach(i => action.Invoke(i, value));
            });

        }

        static void element_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bi = ((TextBox) sender).GetBindingExpression(TextBox.TextProperty);
            if (bi != null)
            {
                bi.UpdateSource();
            }
        }

    #endregion

#region Select all on focus
        public static DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(Extensions), new PropertyMetadata(default(bool)));

        public static bool GetSelectAllOnFocus(FrameworkElement element)
        {
            return (bool) element.GetValue(SelectAllOnFocusProperty);
        }

        public static void SetSelectAllOnFocus(FrameworkElement element, bool value)
        {
            Action<TextBox, bool> action = (textBox, value1) =>
            {
                if (GetSelectAllOnFocus(textBox) == value1) return;

                if (value1)
                {
                    textBox.GotFocus += element_GotFocus;
                }
                else
                {
                    textBox.GotFocus -= element_GotFocus;
                }

                textBox.SetValue(SelectAllOnFocusProperty, value1);
            };

            Deployment.Current.Dispatcher.BeginInvoke(() => { 
                if (element is TextBox)
                {
                    action.Invoke(element as TextBox, value);
                }
                else
                {
                    element.GetChildrens<TextBox>().ForEach(i => action.Invoke(i, value));
                }
            });
        }


        static void element_GotFocus(object sender, RoutedEventArgs e)
        {
            if (GetSelectAllOnFocus(sender as TextBox))
            {
                ((TextBox) sender).SelectAll();
            }
        }

        #endregion

        
    }
}
