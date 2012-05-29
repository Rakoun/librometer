using System;
using System.Windows;
using Microsoft.Phone.Shell;
using WPExtensions;

namespace WPExtensions
{
    public class AdvancedApplicationBarIconButton : BaseAppBarItem, IApplicationBarIconButton
    {
        public AdvancedApplicationBarIconButton()
        {
            AppBarIconButton = new ApplicationBarIconButton() { Text = "appbar" }; 
            RealAppBarItem = AppBarIconButton;
            AdvancedApplicationBar.AddButton(this);
            AppBarIconButton.Click+=AppBarIconButton_Click;
        }

        void AppBarIconButton_Click(object sender, EventArgs e)
        {
            if(DisableLostFocus)
            {
                base.AppBarClick(this, e);
            }
            else
            {
                UIHelper.GetCurrentPage().Focus();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    base.AppBarClick(this, e);
                });
            }
            
        }

        

        public ApplicationBarIconButton AppBarIconButton { get; set; }
        
        public static DependencyProperty IconUriProperty = DependencyProperty.RegisterAttached("IconUri", typeof(Uri), typeof(AdvancedApplicationBarIconButton), new PropertyMetadata(default(Uri), IconUriPropertyChangedCallback));
        

        public static Uri GetIconUri(AdvancedApplicationBarIconButton element)
        {
            return (Uri) element.GetValue(IconUriProperty);
        }

        public static void SetIconUri(AdvancedApplicationBarIconButton element, Uri value)
        {
            element.SetValue(IconUriProperty, value);
        }

        public Uri IconUri
        {
            get { return GetIconUri(this); }
            set { SetIconUri(this,value); }
        }

        private static void IconUriPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdvancedApplicationBarIconButton)d).AppBarIconButton.IconUri = e.NewValue as Uri;
        }


        

       
    }
}
