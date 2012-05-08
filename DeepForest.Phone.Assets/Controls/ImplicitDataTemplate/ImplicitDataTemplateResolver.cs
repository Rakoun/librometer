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

namespace DeepForest.Phone.Assets.Controls
{
    internal static class ImplicitDataTemplateResolver
    {
        internal static DataTemplate Resolve(ContentPresenter contentPresenter)
        {
            return Resolve(contentPresenter, contentPresenter.Content);
        }

        internal static DataTemplate Resolve(ContentControl contentControl)
        {
            return Resolve(contentControl, contentControl.Content);
        }

        private static DataTemplate Resolve(FrameworkElement contentElement, object content)
        {
            DataTemplate resolvedDataTemplate = null;
            if (content != null)
            {
                resolvedDataTemplate = InternalResolve(contentElement, content.GetType().FullName);
            }

            return resolvedDataTemplate;
        }

        private static DataTemplate InternalResolve(FrameworkElement element, string contentTypeName)
        {
            if (element == null)
            {
                return TryFindDataTemplate(Application.Current.Resources, contentTypeName);
            }

            var dataTemplate = TryFindDataTemplate(element.Resources, contentTypeName);
            if (dataTemplate == null)
            {
                var parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
                dataTemplate = InternalResolve(parent, contentTypeName);
            }

            return dataTemplate;
        }

        private static DataTemplate TryFindDataTemplate(ResourceDictionary resourceDictionary, string contentTypeName)
        {
            DataTemplate dataTemplate = null;
            if (resourceDictionary.Contains(contentTypeName))
            {
                dataTemplate = resourceDictionary[contentTypeName] as DataTemplate;
            }

            return dataTemplate;
        }        
    }       
}
