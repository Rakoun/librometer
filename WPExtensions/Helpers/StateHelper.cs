using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Shell;

namespace WPExtensions.Helpers
{
    public class StateHelper
    {
        private static string GetButtonKey()
        {
            var stateKey = "advancedapplicationbar_currentstatebuttons" + UIHelper.GetCurrentPage().ToString();
            return stateKey;
        }

        private static string GetAppBarKey()
        {
            var stateKey = "advancedapplicationbar_appbar" + UIHelper.GetCurrentPage().ToString();
            return stateKey;
        }

        public static void SaveCurrentState(AdvancedApplicationBar advancedApplicationBar)
        {
            var stateList = new List<BaseButton>();
            foreach (var buttonItem in advancedApplicationBar.ButtonItems)
            {
                if(buttonItem is AdvancedApplicationBarMenuItem)
                {
                    var aitem = buttonItem as AdvancedApplicationBarMenuItem;
                    var baseButton = new BaseMenuItemButton()
                    {
                        Text = aitem.Text,
                        IsVisible = aitem.Visibility == Visibility.Visible,
                        IsEnabled = aitem.IsEnabled,
                    };
                    stateList.Add(baseButton);
                }

                if(buttonItem is AdvancedApplicationBarIconButton)
                {
                    var aitem = buttonItem as AdvancedApplicationBarIconButton;
                    var baseButton = new BaseIconItemButton()
                    {
                        Text = aitem.Text,
                        IsVisible = aitem.Visibility == Visibility.Visible,
                        IsEnabled = aitem.IsEnabled,
                        IconUri=aitem.IconUri
                    };
                    stateList.Add(baseButton);
                }
            }
            PhoneApplicationService.Current.State[GetButtonKey()] = stateList;


            PhoneApplicationService.Current.State[GetAppBarKey()] =
                new ApplicationBarSaveStateItem().SaveState(advancedApplicationBar);



        }

        internal static void RestoreIfExistSavedState(AdvancedApplicationBar advancedApplicationBar)
        {
            if (PhoneApplicationService.Current==null) return;
            if (PhoneApplicationService.Current.State.ContainsKey(GetButtonKey()))
            {
                var stateButtons = (PhoneApplicationService.Current.State[GetButtonKey()] as List<BaseButton>);
                if (stateButtons.Count != (advancedApplicationBar.ButtonItems.Count()))
                {
                    return;
                }
                
                for (var i = 0; i < stateButtons.Count; i++)
                {
                    if (advancedApplicationBar.ButtonItems[i] is AdvancedApplicationBarMenuItem)
                    {
                        var abtn = (advancedApplicationBar.ButtonItems[i] as AdvancedApplicationBarMenuItem);
                        abtn.IsEnabled= stateButtons[i].IsEnabled;
                        abtn.Visibility = stateButtons[i].IsVisible ? Visibility.Visible : Visibility.Collapsed;
                        abtn.Text = stateButtons[i].Text;
                    }

                    if (advancedApplicationBar.ButtonItems[i] is AdvancedApplicationBarIconButton)
                    {
                        var abtn = (advancedApplicationBar.ButtonItems[i] as AdvancedApplicationBarIconButton);
                        abtn.IsEnabled = stateButtons[i].IsEnabled;
                        abtn.Visibility = stateButtons[i].IsVisible ? Visibility.Visible : Visibility.Collapsed;
                        abtn.Text = stateButtons[i].Text;
                        abtn.IconUri = ((BaseIconItemButton) stateButtons[i]).IconUri;
                    }
                }

                ((ApplicationBarSaveStateItem) PhoneApplicationService.Current.State[GetAppBarKey()]).RestoreState(advancedApplicationBar);
                PhoneApplicationService.Current.State.Remove(GetButtonKey());
            }
            
        }

    }
}
