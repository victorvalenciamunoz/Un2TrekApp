using Un2TrekApp.Controls;

namespace Un2TrekApp.Helpers;

public class FlyoutHelper
{
    public static void AddFlyoutMenusDetails()
    {
        AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
        AppShell.Current.Items.Clear();

        if (App.UserInfo.UserRoles == App.RoleAdministrator)
        {
            var flyoutItem = new FlyoutAdminItemsControl();
            flyoutItem.Route = nameof(ActivityListPage);
            flyoutItem.FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems;
            if (!AppShell.Current.Items.Contains(flyoutItem))
            {
                AppShell.Current.Items.Add(flyoutItem);
            }
        }
        else
        {
            var flyoutItem = new FlyoutItemsControl();
            flyoutItem.Route = nameof(ActivityListPage);
            flyoutItem.FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems;
            if (!AppShell.Current.Items.Contains(flyoutItem))
            {
                AppShell.Current.Items.Add(flyoutItem);
            }
        }
    }
}
