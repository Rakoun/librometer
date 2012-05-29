using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace WPExtensions
{
    public partial class Extensions
    {


        public static DependencyProperty FocusOnLoadProperty = DependencyProperty.RegisterAttached("FocusOnLoad", typeof (string), typeof (Extensions), new PropertyMetadata(String.Empty));
        

        public static String GetFocusOnLoad(Page element)
        {
            return (String) element.GetValue(FocusOnLoadProperty);
        }

        public static void SetFocusOnLoad(Page element, string value)
        {
            if (GetFocusOnLoad(element) != value)
            {
                element.Loaded += (sender, e) =>
                {
                    var textBox = UIHelper.FindChildItem(element, value) as TextBox;
                    element.SetValue(FocusOnLoadProperty, value);
                    textBox.Focus();
                };
            }
        }

        //public static DependencyProperty AnimationOnChangeOrientationProperty = DependencyProperty.RegisterAttached("AnimationOnChangeOrientation", typeof(bool), typeof(Extensions), new PropertyMetadata(default(bool)));

        //public static bool GetAnimationOnChangeOrientation(PhoneApplicationPage element)
        //{
        //    return (bool) element.GetValue(AnimationOnChangeOrientationProperty);
        //}

        //public static void SetAnimationOnChangeOrientation(PhoneApplicationPage element, bool value)
        //{
        //    if (GetAnimationOnChangeOrientation(element) != value)
        //    {
        //        if (value)
        //        {
        //            lastOrientation = element.Orientation;
        //            element.OrientationChanged += element_OrientationChanged;
        //        }
        //        else
        //        {
        //            element.OrientationChanged -= element_OrientationChanged;
        //        }
        //        element.SetValue(AnimationOnChangeOrientationProperty, value);    
        //    }
        //}

        //private static PageOrientation lastOrientation;

        //static void element_OrientationChanged(object sender, OrientationChangedEventArgs e)
        //{
        //    //PageOrientation lastOrientation = ((PhoneApplicationPage)sender).Orientation;
        //    PageOrientation newOrientation = e.Orientation;
            
        //    // Orientations are (clockwise) 'PortraitUp', 'LandscapeRight', 'LandscapeLeft'

        //    RotateTransition transitionElement = new RotateTransition();

        //    switch (newOrientation)
        //    {
        //        case PageOrientation.Landscape:
        //        case PageOrientation.LandscapeRight:
        //            // Come here from PortraitUp (i.e. clockwise) or LandscapeLeft?
        //            if (lastOrientation == PageOrientation.PortraitUp)
        //                transitionElement.Mode = RotateTransitionMode.In90Counterclockwise;
        //            else
        //                transitionElement.Mode = RotateTransitionMode.In180Clockwise;
        //            break;
        //        case PageOrientation.LandscapeLeft:
        //            // Come here from LandscapeRight or PortraitUp?
        //            if (lastOrientation == PageOrientation.LandscapeRight)
        //                transitionElement.Mode = RotateTransitionMode.In180Counterclockwise;
        //            else
        //                transitionElement.Mode = RotateTransitionMode.In90Clockwise;
        //            break;
        //        case PageOrientation.Portrait:
        //        case PageOrientation.PortraitUp:
        //            // Come here from LandscapeLeft or LandscapeRight?
        //            if (lastOrientation == PageOrientation.LandscapeLeft)
        //                transitionElement.Mode = RotateTransitionMode.In90Counterclockwise;
        //            else
        //                transitionElement.Mode = RotateTransitionMode.In90Clockwise;
        //            break;
        //        default:
        //            break;
        //    }

        //    // Execute the transition
        //    PhoneApplicationPage phoneApplicationPage = (PhoneApplicationPage)(((PhoneApplicationFrame)Application.Current.RootVisual)).Content;
        //    ITransition transition = transitionElement.GetTransition(phoneApplicationPage);
        //    transition.Completed += delegate
        //    {
        //        transition.Stop();
        //    };
        //    transition.Begin();
        //    lastOrientation = newOrientation;
        //}
    }
}