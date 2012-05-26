using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using WindowsPhoneApplicationTest.ViewModels;

namespace WindowsPhoneApplicationTest {
    public partial class MainPage : PhoneApplicationPage {
        private ApplicationBar applicationBarChoose;
        private ApplicationBarIconButton applicationBarIconButtonChoose;

        private ApplicationBar applicationBarDeleteOrCancel;
        private ApplicationBarIconButton applicationBarIconButtonDelete;
        private ApplicationBarIconButton applicationBarIconButtonCancel;


        // Constructor
        public MainPage() {
            InitializeComponent();

            ConstructUI();

            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        public void ConstructUI() {
            // Application Bar
            this.applicationBarChoose = new ApplicationBar();
            this.applicationBarIconButtonChoose = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Choose.png", UriKind.Relative));
            this.applicationBarIconButtonChoose.Text = "choose";
            this.applicationBarIconButtonChoose.Click += new EventHandler(applicationBarIconButtonChoose_Click);
            this.applicationBarChoose.Buttons.Add(this.applicationBarIconButtonChoose);
            this.applicationBarChoose.IsMenuEnabled = true;
            this.applicationBarChoose.IsVisible = true;
            this.ApplicationBar = this.applicationBarChoose;

            this.applicationBarDeleteOrCancel = new ApplicationBar();
            this.applicationBarIconButtonDelete = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Delete.png", UriKind.Relative));
            this.applicationBarIconButtonDelete.Text = "delete";
            this.applicationBarIconButtonDelete.Click += new EventHandler(applicationBarIconButtonDelete_Click);
            this.applicationBarIconButtonCancel = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Cancel.png", UriKind.Relative));
            this.applicationBarIconButtonCancel.Text = "cancel";
            this.applicationBarIconButtonCancel.Click += new EventHandler(applicationBarIconButtonCancel_Click);
            this.applicationBarDeleteOrCancel.Buttons.Add(this.applicationBarIconButtonDelete);
            this.applicationBarDeleteOrCancel.Buttons.Add(this.applicationBarIconButtonCancel);
            this.applicationBarDeleteOrCancel.IsMenuEnabled = true;
            this.applicationBarDeleteOrCancel.IsVisible = true;
        }

        private void SwitchToChooseState() {
            this.listBoxWithCheckBoxes.IsInChooseState = true;
            this.ApplicationBar = this.applicationBarDeleteOrCancel;
        }

        private void SwitchToNormalState() {
            this.listBoxWithCheckBoxes.IsInChooseState = false;
            this.ApplicationBar = this.applicationBarChoose;
        }



        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e) {
            if (!App.ViewModel.IsDataLoaded) {
                App.ViewModel.LoadData();
            }
        }

        private void applicationBarIconButtonDelete_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Are you sure to delete these items?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK) {
                foreach (SimpleModel aSimpleModel in this.listBoxWithCheckBoxes.SelectedItems) {
                    App.ViewModel.SimpleModels.Remove(aSimpleModel);
                }
                SwitchToNormalState();
            }
        }

        private void applicationBarIconButtonChoose_Click(object sender, EventArgs e) {
            SwitchToChooseState();
        }

        private void applicationBarIconButtonCancel_Click(object sender, EventArgs e) {
            SwitchToNormalState();
        }
    }
}