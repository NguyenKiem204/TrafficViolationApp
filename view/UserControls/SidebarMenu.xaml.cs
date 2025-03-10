﻿using System;
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
                btnViolations,
                btnVehicles,
                btnUsers,
                btnNotifications,
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
