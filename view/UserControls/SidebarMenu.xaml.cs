using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrafficViolationApp.view.UserControls
{
    /// <summary>
    /// Interaction logic for SidebarMenu.xaml
    /// </summary>
    public partial class SidebarMenu : UserControl
    {
        public event EventHandler<MenuItemSelectedEventArgs> MenuItemSelected;

        public event EventHandler LogoutClicked;

        private Dictionary<string, List<string>> RoleMenuVisibility = new Dictionary<string, List<string>>
    {
        { "Admin", new List<string>
            {
                "Dashboard",
                "Reports",
                "Violations",
                "SendViolions",
                "Vehicles",
                "Users",
                "Notifications",
                "ManagerReport",
                "ManagerNotification",
                "Settings"
            }
        },
        { "TrafficPolice", new List<string>
            {
                "Dashboard",
                "SendViolions",
                "Users",
                "ManagerReport",
                "ManagerNotification",
                "Settings"
            }
        },
        { "Citizen", new List<string>
            {
                "Dashboard",
                "SendViolions",
                "Settings"
            }
        }
    };
        public SidebarMenu()
        {
            InitializeComponent();
        }
        public void SetActiveMenuItem(string menuItem)
        {
            foreach (var button in GetMenuButtons())
            {
                button.Background = System.Windows.Media.Brushes.Transparent;
            }
            Button activeButton = FindButtonByTag(menuItem);
            if (activeButton != null)
            {
                activeButton.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#3D51A9");
            }
        }

        public void SetUserInfo(string userName, string userRole, string initials)
        {
            txtUserName.Text = userName;
            txtUserRole.Text = userRole;
            txtUserInitials.Text = initials;

            ApplyRoleBasedMenuVisibility(userRole);
        }

        private void ApplyRoleBasedMenuVisibility(string userRole)
        {
            List<string> visibleMenuItems = RoleMenuVisibility.ContainsKey(userRole)
                ? RoleMenuVisibility[userRole]
                : new List<string>();

            Button[] menuButtons = GetMenuButtons();

            foreach (var button in menuButtons)
            {
                string menuItem = button.Tag.ToString();
                button.Visibility = visibleMenuItems.Contains(menuItem)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string menuItem = button.Tag.ToString();
                SetActiveMenuItem(menuItem);
                MenuItemSelected?.Invoke(this, new MenuItemSelectedEventArgs(menuItem));
            }
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutClicked?.Invoke(this, EventArgs.Empty);
        }
        private Button[] GetMenuButtons()
        {
            return new Button[]
            {
                btnDashboard,
                btnReports,
                btnSendViolions,
                btnViolations,
                btnVehicles,
                btnUsers,
                btnNotifications,
                btnManagerNoti,
                btnManagerReport,
                btnSettings
            };
        }
        private Button FindButtonByTag(string tag)
        {
            foreach (var button in GetMenuButtons())
            {
                if (button.Tag.ToString() == tag)
                {
                    return button;
                }
            }
            return null;
        }
    }

    public class MenuItemSelectedEventArgs : EventArgs
    {
        public string MenuItem { get; private set; }

        public MenuItemSelectedEventArgs(string menuItem)
        {
            MenuItem = menuItem;
        }
    }
}
