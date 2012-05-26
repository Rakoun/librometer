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
    [TemplateVisualState(Name = "NormalState", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "ChooseState", GroupName = "CommonStates")]
    public class ListBoxWithCheckBoxes : ListBox {

        public static readonly DependencyProperty MinWidthForChooseStateProperty = DependencyProperty.Register(
            "MinWidthForChooseState",
            typeof(double),
            typeof(ListBoxWithCheckBoxes),
            new PropertyMetadata(new double(), OnMinWidthForChooseStatePropertyChanged));

        public static readonly DependencyProperty IsInChooseStateProperty = DependencyProperty.Register(
            "IsInChooseState",
            typeof(bool),
            typeof(ListBoxWithCheckBoxes),
            new PropertyMetadata(false, OnIsInChooseStatePropertyChanged));



        public static double GetMinWidthForChooseState(DependencyObject theTarget) {
            return (double)theTarget.GetValue(MinWidthForChooseStateProperty);
        }

        public static void SetMinWidthForChooseState(DependencyObject theTarget, double theMinWidthForChooseState) {
            theTarget.SetValue(MinWidthForChooseStateProperty, theMinWidthForChooseState);
        }



        public double MinWidthForChooseState {
            get { return (double)GetValue(MinWidthForChooseStateProperty); }
            set { SetValue(MinWidthForChooseStateProperty, value); }
        }
        
        public bool IsInChooseState {
            get { return (bool)GetValue(IsInChooseStateProperty); }
            set { SetValue(IsInChooseStateProperty, value); }
        }



        private static void OnMinWidthForChooseStatePropertyChanged(DependencyObject theTarget, DependencyPropertyChangedEventArgs theDependencyPropertyChangedEventArgs) {
            // Do nothing
        }

        private static void OnIsInChooseStatePropertyChanged(DependencyObject theTarget, DependencyPropertyChangedEventArgs theDependencyPropertyChangedEventArgs) {
            if (theTarget != null && theTarget is ListBoxWithCheckBoxes) {
                ListBoxWithCheckBoxes aListBoxWithCheckBoxes = (ListBoxWithCheckBoxes)theTarget;
                if ((bool)theDependencyPropertyChangedEventArgs.NewValue) {
                    aListBoxWithCheckBoxes.SelectionMode = Controls.SelectionMode.Multiple;
                    aListBoxWithCheckBoxes.SelectedItems.Clear();
                    VisualStateManager.GoToState(aListBoxWithCheckBoxes, "ChooseState", true);
                } else {
                    aListBoxWithCheckBoxes.SelectionMode = Controls.SelectionMode.Single;
                    VisualStateManager.GoToState(aListBoxWithCheckBoxes, "NormalState", true);
                }
            }
        }



        public ListBoxWithCheckBoxes()
            : base() {
            this.DefaultStyleKey = typeof(ListBoxWithCheckBoxes);
            this.SelectionMode = Controls.SelectionMode.Single;

            this.Loaded += new RoutedEventHandler(ListBoxWithCheckBoxes_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(ListBoxWithCheckBoxes_SizeChanged);
        }



        void ListBoxWithCheckBoxes_SizeChanged(object sender, SizeChangedEventArgs e) {
            SetMinWidthForChooseState(this, this.ActualWidth);
        }

        void ListBoxWithCheckBoxes_Loaded(object sender, RoutedEventArgs e) {
            SetMinWidthForChooseState(this, this.ActualWidth);
        }
    }
}
