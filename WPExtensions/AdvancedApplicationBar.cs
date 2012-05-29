using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using WPExtensions;
using WPExtensions.Helpers;

namespace WPExtensions
{
    [ContentProperty("Items")]
    public class AdvancedApplicationBar : ItemsControl, IApplicationBar
    {
        public const ApplicationBarMode DefaultAppBarMode = ApplicationBarMode.Default;

        public AdvancedApplicationBar()
        {
            if (PhoneApplicationService.Current != null)
            {
                PhoneApplicationService.Current.Deactivated -= Current_Deactivated;
                PhoneApplicationService.Current.Activated -= Current_Activated;
                PhoneApplicationService.Current.Deactivated += Current_Deactivated;
                PhoneApplicationService.Current.Activated += Current_Activated;
            }
            
            Loaded += AdvancedApplicationBar_Loaded;

            
        }

        void Current_Activated(object sender, ActivatedEventArgs e)
        {
            StateHelper.RestoreIfExistSavedState(this);
        }

        void Current_Deactivated(object sender, DeactivatedEventArgs e)
        {
            StateHelper.SaveCurrentState(this);
        }


        

        void AdvancedApplicationBar_Loaded(object sender, RoutedEventArgs e)
        {

            Binding myBinding = new Binding("Opacity");
            myBinding.Source = this;
            myBinding.Mode = BindingMode.TwoWay;
            
            this.SetBinding(BindableOpacityProperty, myBinding);


            ReCreateAppBar();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems.Count > 0)
                {
                    if (e.NewItems[0] is IAppBarItem)
                    {
                        var appBarItem = e.NewItems[0] as IAppBarItem;
                        appBarItem.SetParentApplicationBar(this);
                        buttonItems.Add(appBarItem);
                    }
                }
            }
        }

        private List<IAppBarItem> buttonItems = new List<IAppBarItem>();
        public List<IAppBarItem> ButtonItems
        {
            get
            {
                var index = 0;
                if (AdvancedButtons.Count > 0)
                {
                    buttonItems.Clear();
                    AdvancedButtons.ForEach(i =>
                    {
                        ((BaseAppBarItem) i).Id = index;
                        i.SetParentApplicationBar(this);
                        buttonItems.Add(i);
                        index++;
                    });
                    AdvancedButtons.Clear();

                    StateHelper.RestoreIfExistSavedState(this);
                }
                return buttonItems;
            }
            
        }

        

        public void ReCreateAppBar()
        {

            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }

            GetApplicationBar().Buttons.Clear();
            GetApplicationBar().MenuItems.Clear();
            var buttonsCount = 0;
            var menuItemsCount = 0;
            foreach (var item in ButtonItems)
            {
                if (item is AdvancedApplicationBarIconButton)
                {
                    var advancedApplicationBarIconButton = (AdvancedApplicationBarIconButton) item;
                    if (advancedApplicationBarIconButton.Visibility == Visibility.Visible &&
                        advancedApplicationBarIconButton.ShouldBeDraw)
                    {
                        GetApplicationBar().Buttons.Add(advancedApplicationBarIconButton.AppBarIconButton);
                        buttonsCount++;
                    }

                }
                else if (item is AdvancedApplicationBarMenuItem)
                {
                    var advancedApplicationBarMenuItem = (AdvancedApplicationBarMenuItem)item;
                    if (advancedApplicationBarMenuItem.Visibility == Visibility.Visible &&
                        advancedApplicationBarMenuItem.ShouldBeDraw)
                    {
                        GetApplicationBar().MenuItems.Add(advancedApplicationBarMenuItem.ApplicationBarMenuItem);
                        menuItemsCount++;
                    }
                }
            }

            GetApplicationBar().IsVisible = this.IsVisible;
            if ((buttonsCount+menuItemsCount) == 0)
            {
                GetApplicationBar().IsVisible = EmptyAppBarIsVisible;
            }
            
            GetApplicationBar().Mode = buttonsCount == 0
                                               ? ApplicationBarMode.Minimized
                                               : this.Mode;
            

        }

        private bool appBarInitiated;
        public IApplicationBar GetApplicationBar()
        {
            var appBar= UIHelper.GetCurrentPage().ApplicationBar ??
                   (UIHelper.GetCurrentPage().ApplicationBar = new ApplicationBar());
            if (!appBarInitiated)
            {
                appBar.Mode = DefaultAppBarMode;
                appBarInitiated = true;
            }
            return appBar;
        }

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(AdvancedApplicationBar), new PropertyMetadata(true, OnVisibleChanged));

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((AdvancedApplicationBar)d).GetApplicationBar().IsVisible= (bool)e.NewValue;
            }
        }

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        

        public IList Buttons { get; set; }

        public IList MenuItems { get; set; }

        private static readonly List<IAppBarItem> AdvancedButtons = new List<IAppBarItem>();

        public static void AddButton(IAppBarItem button)
        {
            AdvancedButtons.Add(button);
        }

        public static DependencyProperty IsMenuEnabledProperty = DependencyProperty.Register("IsMenuEnabled", typeof(bool), typeof(AdvancedApplicationBar), new PropertyMetadata(true, IsMenuEnabledChanged));
        private static void IsMenuEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((AdvancedApplicationBar)d).GetApplicationBar().IsMenuEnabled= (bool)e.NewValue;
            }
        }

        public bool IsMenuEnabled
        {
            get { return (bool)GetValue(IsMenuEnabledProperty); }
            set { SetValue(IsMenuEnabledProperty, value); }
        }


        public new double Opacity
        {
            get { return base.Opacity; }
            set
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.GetApplicationBar().Opacity = (double)value;
                });

                base.Opacity = value;

            }
        }

        public static DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(AdvancedApplicationBar), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0), BackgroundColorChanged));
        private static void BackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { 
                    ((AdvancedApplicationBar)d).GetApplicationBar().BackgroundColor = (Color)e.NewValue;
                });
            }
        }
        public Color BackgroundColor
        {
            get { return (Color) GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }


        public static DependencyProperty ForegroundColorProperty = DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(AdvancedApplicationBar), new PropertyMetadata(Color.FromArgb(0,0,0,0), ForegroundColorChanged));
        private static void ForegroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { 
                    ((AdvancedApplicationBar)d).GetApplicationBar().ForegroundColor = (Color)e.NewValue;
                });
            }
        }
        public Color ForegroundColor
        {
            get { return (Color) GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        public static DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(ApplicationBarMode), typeof(AdvancedApplicationBar), new PropertyMetadata(DefaultAppBarMode, ModeChanged));
        

        private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ((AdvancedApplicationBar) d).GetApplicationBar().Mode = (ApplicationBarMode) e.NewValue;
                });
            }
        }

        public ApplicationBarMode Mode
        {
            get { return (ApplicationBarMode) GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public double DefaultSize
        {
            get
            {
                return GetApplicationBar().DefaultSize;
            }
        }

        public double MiniSize
        {
            get { return GetApplicationBar().MiniSize; }
        }

        public static DependencyProperty EmptyAppBarIsVisibleProperty = DependencyProperty.Register("EmptyAppBarIsVisible", typeof(bool), typeof(AdvancedApplicationBar), new PropertyMetadata(default(bool)));
        public bool EmptyAppBarIsVisible
        {
            get { return (bool) GetValue(EmptyAppBarIsVisibleProperty); }
            set { SetValue(EmptyAppBarIsVisibleProperty, value); }
        }


        public static DependencyProperty BindableOpacityProperty = DependencyProperty.Register("BindableOpacity", typeof(double), typeof(AdvancedApplicationBar), new PropertyMetadata(default(double), BindableOpacityChanged));
        public double BindableOpacity
        {
            get { return (double) GetValue(BindableOpacityProperty); }
            set { SetValue(BindableOpacityProperty, value); }
        }

        private static void BindableOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ((AdvancedApplicationBar)d).GetApplicationBar().Opacity = (double)e.NewValue;
                });
            }
        }

        public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;


    }
}
