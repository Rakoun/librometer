//===============================================================================
// Microsoft patterns & practices
// Developing a Windows Phone Application using the MVVM Pattern
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

namespace Librometer.Adapters
{
    using System;
    using Microsoft.Phone.Controls;

    public class NavigationServiceFacade: INavigationServiceFacade
    {
        /*RGE readonly*/private PhoneApplicationFrame _frame;
        public PhoneApplicationFrame Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in the back navigation history.
        /// </summary>
        public bool CanGoBack
        {
            get { return _frame.CanGoBack; }
        }

        /// <summary>
        /// Gets the uniform resource identifier (URI) of the content that is currently displayed.
        /// </summary>
        public Uri CurrentSource
        {
            get { return _frame.CurrentSource; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationServiceFacade"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">If frame is null.</exception>
        /// <param name="frame">The frame.</param>
        public NavigationServiceFacade(PhoneApplicationFrame frame)
        {
            if (frame == null) throw new ArgumentNullException("frame");
            this._frame = frame;
        }

        /// <summary>
        /// Navigates to the content specified by the uniform resource identifier (URI).
        /// </summary>
        /// <param name="source">The URI of the content to navigate to.</param>
        /// <returns>Returns bool. True if the navigation started successfully; otherwise, false.</returns>
        public bool Navigate(Uri source)
        {
            return _frame.Navigate(source);
        }

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or throws an exception if no entry exists in back navigation.
        /// </summary>
        public void GoBack()
        {
            _frame.GoBack();
        }
    }
}
