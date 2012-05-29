using System.Windows.Media;
using Microsoft.Phone.Shell;

namespace WPExtensions.Helpers
{
    public class ApplicationBarSaveStateItem
    {
        public ApplicationBarSaveStateItem()
        {
        }

        public ApplicationBarSaveStateItem SaveState(AdvancedApplicationBar applicationBar)
        {
            IsVisible = applicationBar.IsVisible;
            IsMenuEnabled = applicationBar.IsMenuEnabled;
            BackgroundColor = applicationBar.BackgroundColor;
            ForegroundColor = applicationBar.ForegroundColor;
            Mode = applicationBar.Mode;
            Opacity = applicationBar.Opacity;
            return this;
        }

        public AdvancedApplicationBar RestoreState(AdvancedApplicationBar applicationBar)
        {
            applicationBar.IsVisible = IsVisible;
            applicationBar.IsMenuEnabled = IsMenuEnabled;
            applicationBar.BackgroundColor = BackgroundColor;
            applicationBar.ForegroundColor = ForegroundColor;
            applicationBar.Mode = Mode;
            applicationBar.Opacity = Opacity;
            return applicationBar;
        }

        public bool IsVisible { get; set; }
        public bool IsMenuEnabled { get; set; }
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public ApplicationBarMode Mode { get; set; }
        public double Opacity { get; set; }
    }
}