using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WPExtensions;

namespace WPExtensions
{
    public abstract class BaseAppBarItem: FrameworkElement, IAppBarItem
    {
        public IApplicationBarMenuItem RealAppBarItem { get; set; }

        public int Id { get; set; }

        public AdvancedApplicationBar ParentAdvancedApplicationBar { get; set; }
        public void SetParentApplicationBar(AdvancedApplicationBar advancedApplicationAppBar)
        {
            ParentAdvancedApplicationBar = advancedApplicationAppBar;
        }

        private Panorama currentPanorama;
        private PanoramaItem currentPanoramaItem;

        private Pivot currentPivot;
        private PivotItem currentPivotItem;

        private bool shouldBeDrawAlreadyCalled = false;

        public bool ShouldBeDraw
        {
            get
            {
                if (!shouldBeDrawAlreadyCalled)
                {
                    shouldBeDrawAlreadyCalled = true;
                    if (!(this.Parent != null && this.Parent is AdvancedApplicationBar))
                    {
                        currentPanoramaItem = UIHelper.GetParent<PanoramaItem>(this.Parent as FrameworkElement);
                        if (currentPanoramaItem != null)
                        {
                            currentPanorama = UIHelper.GetParent<Panorama>(this.Parent as FrameworkElement);
                            currentPanorama.SelectionChanged += currentPanorama_SelectionChanged;
                        }

                        currentPivotItem = UIHelper.GetParent<PivotItem>(this.Parent as FrameworkElement);
                        if (currentPivotItem != null)
                        {
                            currentPivot = UIHelper.GetParent<Pivot>(this.Parent as FrameworkElement);
                            currentPivot.SelectionChanged += currentPivot_SelectionChanged;
                        }
                    }
                }

                if (currentPanorama != null)
                {
                    return currentPanorama.SelectedItem == currentPanoramaItem;
                }

                if (currentPivot != null)
                {
                    return currentPivot.SelectedItem == currentPivotItem;
                }

                return true;
            }
        }

        void currentPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateAppBar();
        }

        void currentPanorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateAppBar();
        }

        private void CreateAppBar()
        {
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }
            if (ParentAdvancedApplicationBar!=null)
                ParentAdvancedApplicationBar.ReCreateAppBar();
        }

        public static DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(AdvancedApplicationBarIconButton), new PropertyMetadata(true, IsEnabledPropertyChangedEventHandler));
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        private static void IsEnabledPropertyChangedEventHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BaseAppBarItem)d).RealAppBarItem.IsEnabled = (bool)e.NewValue;
            }
        }

        public static DependencyProperty DisableLostFocusProperty = DependencyProperty.Register("DisableLostFocus", typeof(bool), typeof(AdvancedApplicationBarIconButton), new PropertyMetadata(false, DisableLostFocusChangedCallback));
        public bool DisableLostFocus
        {
            get { return (bool)GetValue(DisableLostFocusProperty); }
            set { SetValue(DisableLostFocusProperty, value); }
        }

        private static void DisableLostFocusChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BaseAppBarItem) d).DisableLostFocus = (bool)e.NewValue;
            }
        }




        public static DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string), typeof(AdvancedApplicationBarIconButton), new PropertyMetadata("appBar", TextPropertyChangedCallback));
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BaseAppBarItem)d).RealAppBarItem.Text = e.NewValue.ToString();
            }
        }

        public new static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), typeof(AdvancedApplicationBarIconButton), new PropertyMetadata(OnVisibilityChanged));

        public new Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                var button = ((AdvancedApplicationBarIconButton)d);
                if (button.ParentAdvancedApplicationBar!=null)
                    button.ParentAdvancedApplicationBar.ReCreateAppBar();

            }
        }


        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(BaseAppBarItem), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(BaseAppBarItem), null);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        protected void AppBarClick(object sender,EventArgs e )
        {
            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }

            if (Click != null)
            {
                Click(this, e);
            }
        }

        public event EventHandler Click;
    }
}