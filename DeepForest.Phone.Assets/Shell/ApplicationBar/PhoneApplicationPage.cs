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

namespace DeepForest.Phone.Assets.Shell
{
    public static class PhoneApplicationPage
    {
        public static DeepForest.Phone.Assets.Shell.ApplicationBar GetApplicationBar(Microsoft.Phone.Controls.PhoneApplicationPage obj)
        {
            return (DeepForest.Phone.Assets.Shell.ApplicationBar)obj.GetValue(ApplicationBarProperty);
        }

        public static void SetApplicationBar(Microsoft.Phone.Controls.PhoneApplicationPage obj, DeepForest.Phone.Assets.Shell.ApplicationBar value)
        {
            obj.SetValue(ApplicationBarProperty, value);
        }

        // Using a DependencyProperty as the backing store for ApplicationBar.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty ApplicationBarProperty =
            DependencyProperty.RegisterAttached(
                "ApplicationBar",
                typeof(DeepForest.Phone.Assets.Shell.ApplicationBar),
                typeof(PhoneApplicationPage),
                new PropertyMetadata(null, ApplicationBarPropertyChanged));

        private static void ApplicationBarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as Microsoft.Phone.Controls.PhoneApplicationPage;
            if (e.NewValue != null)
            {                
                var appBar = e.NewValue as ApplicationBar;
                page.ApplicationBar = appBar.SysAppBar;
            }
            else
            {

            }
        }
    }
}
