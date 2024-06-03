using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Helper
{
    public class MenuBarHandler
    {
        public static MenuBarHandler Instance { get; } = new MenuBarHandler();

        public event EventHandler<MenuFlyoutItemClickedEventArgs> MenuFlyoutItemClicked;

        protected virtual void OnMenuFlyoutItemClicked(MenuFlyoutItemClickedEventArgs e)
        {
            MenuFlyoutItemClicked?.Invoke(this, e);
        }

        public void HandleMenuFlyoutItemClick(MenuFlyoutItem item, INavigation navigationService)
        {
            OnMenuFlyoutItemClicked(new MenuFlyoutItemClickedEventArgs(item, navigationService));
        }
    }


    public class MenuFlyoutItemClickedEventArgs : EventArgs
    {
        public MenuFlyoutItem Item { get; }
        public INavigation NavigationService { get; }

        public MenuFlyoutItemClickedEventArgs(MenuFlyoutItem item, INavigation navigationService)
        {
            Item = item;
            NavigationService = navigationService;
        }
    }

}
