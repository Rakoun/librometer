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

using Librometer.Adapters;

namespace Librometer.Framework
{
    public partial class SaveOrCancelPage : PhoneApplicationPage
    {
        public SaveOrCancelPage()
        {
            InitializeComponent();

        }

        #region CloseReason

        /// <summary>
        /// CloseReason Dependency Property
        /// </summary>
        public static readonly DependencyProperty CloseReasonProperty =
            DependencyProperty.Register("RaisonDeFermeture", typeof(MessageBoxResult), typeof(SaveOrCancelPage),
                /*RGE new FrameworkPropertyMetadata(MessageBoxResult.OK)*/
                new PropertyMetadata(MessageBoxResult.None));

        /// <summary>
        /// Gets or sets the CloseReason property. This dependency property 
        /// indicates ....
        /// </summary>
        public MessageBoxResult CloseReason
        {
            get { return (MessageBoxResult)GetValue(CloseReasonProperty); }
            set { SetValue(CloseReasonProperty, value); }
        }

        #endregion //CloseReason

        #region Evènements

        private void AppBarIconBtnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void AppBarIconBtnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void AppBarMnuItemSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void AppBarMnuItemCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        #endregion //Evènements

        #region Méthodes privées

        private void Save()
        {
            CloseReason = MessageBoxResult.OK;
            if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
        }

        private void Cancel()
        {
            CloseReason = MessageBoxResult.Cancel;
            if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();//TODO: voir s'il n'y a pas mieux
            VisualStateManager.GoToState(this, "HasErrors", true);
        }

        #endregion //Méthodes privées
    }
}