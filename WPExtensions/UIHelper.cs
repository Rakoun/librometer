using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Phone.Controls;

namespace WPExtensions
{
    public static class UIHelper
    {
        public static PhoneApplicationPage GetCurrentPage()
        {
            if (DesignerProperties.IsInDesignTool)
            {
                return new PhoneApplicationPage();
            }
            var root = ((Application.Current).RootVisual);
            if (root is PhoneApplicationFrame)
                return (PhoneApplicationPage)((PhoneApplicationFrame)(root)).Content;
            return  GetPageOfElement((FrameworkElement) root);
        }

        public static PhoneApplicationPage GetPageOfElement(FrameworkElement element)
        {
            while (!(element is PhoneApplicationPage))
            {
                element = element.Parent as FrameworkElement;
            }
            return element as PhoneApplicationPage;
        }

        public static FrameworkElement FindChildItem(DependencyObject root, string elementName)
        {
            for (var i=0;i<VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                if (((FrameworkElement) child).Name == elementName)
                {
                    return (FrameworkElement) child;
                }
                var childItem = FindChildItem(child, elementName);
                if (childItem != null) return childItem;
            }
            
            return null;
        }

        public static void DispatcherFocus(Control control)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => control.Focus());
        }

        public static T GetParent<T>(FrameworkElement frameworkElement) where T:FrameworkElement
        {
            if(frameworkElement.Parent==null)
                return null;
            if (frameworkElement.Parent is T)
            {
                return frameworkElement.Parent as T;
            }
            
            return GetParent<T>(frameworkElement.Parent as FrameworkElement);
        }

        public static List<T> GetChildrens<T>(this FrameworkElement ele, Func<DependencyObject, bool> whereFunc = null) where T : class
        {
            if (ele == null)
                return null;
            var output = new List<T>();
            var c = VisualTreeHelper.GetChildrenCount(ele);
            for (var i = 0; i < c; i++)
            {
                var ch = VisualTreeHelper.GetChild(ele, i);
                if (whereFunc != null)
                {
                    if (!whereFunc(ch))
                    {
                        continue;
                    }
                }
                if ((ch is T))
                    output.Add(ch as T);
                if (!(ch is FrameworkElement))
                    continue;

                output.AddRange((ch as FrameworkElement).GetChildrens<T>(whereFunc));
            }
            return output;
        }

    }
}