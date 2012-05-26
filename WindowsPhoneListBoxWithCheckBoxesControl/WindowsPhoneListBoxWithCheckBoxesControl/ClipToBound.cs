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

namespace System.Windows.Controls {
    public class ClipToBound {

        public static readonly DependencyProperty ClipToBoundsProperty = DependencyProperty.RegisterAttached(
            "ClipToBounds", 
            typeof(bool),
            typeof(ClipToBound), 
            new PropertyMetadata(false, OnToBoundsPropertyChanged));



        public static bool GetClipToBounds(DependencyObject theTarget) {
            return (bool)theTarget.GetValue(ClipToBoundsProperty);
        }

        public static void SetClipToBounds(DependencyObject theTarget, bool theClipToBounds) {
            theTarget.SetValue(ClipToBoundsProperty, theClipToBounds);
        }



        private static void OnToBoundsPropertyChanged(DependencyObject theTarget, DependencyPropertyChangedEventArgs theDependencyPropertyChangedEventArgs) {
            if (theTarget != null && theTarget is FrameworkElement) {
                FrameworkElement aFrameworkElement = (FrameworkElement)theTarget;
                ClipToBounds(aFrameworkElement);

                aFrameworkElement.Loaded += new RoutedEventHandler(FrameworkElement_Loaded);
                aFrameworkElement.SizeChanged += new SizeChangedEventHandler(FrameworkElement_SizeChanged);
            }
        }

        private static void ClipToBounds(FrameworkElement theTarget) {
            if (GetClipToBounds(theTarget)) {
                RectangleGeometry aRectangleGeometry = new RectangleGeometry();
                aRectangleGeometry.Rect = new Rect(0, 0, theTarget.ActualWidth, theTarget.ActualHeight);
                theTarget.Clip = aRectangleGeometry;
            } else {
                theTarget.Clip = null;
            }
        }



        static void FrameworkElement_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (sender != null && sender is FrameworkElement) {
                ClipToBounds((FrameworkElement)sender);
            }
        }

        static void FrameworkElement_Loaded(object sender, RoutedEventArgs e) {
            if (sender != null && sender is FrameworkElement) {
                ClipToBounds((FrameworkElement)sender);
            }
        }
    }
}
