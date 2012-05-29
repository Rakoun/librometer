using WPExtensions;

namespace WPExtensions
{
    public interface IAppBarItem
    {
        void SetParentApplicationBar(AdvancedApplicationBar advancedApplicationBar);
        bool ShouldBeDraw { get; }
    }
}