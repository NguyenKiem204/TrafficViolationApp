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
using System.Windows.Shapes;
using TrafficViolationApp.view.UserControls;

namespace TrafficViolationApp
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            sidebarMenu.SetActiveMenuItem("Dashboard");
            sidebarMenu.SetUserInfo("Admin User", "Administrator", "AU");
            sidebarMenu.MenuItemSelected += SidebarMenu_MenuItemSelected;
            sidebarMenu.LogoutClicked += SidebarMenu_LogoutClicked;

        }
        private void SidebarMenu_MenuItemSelected(object sender, MenuItemSelectedEventArgs e)
        {
            switch (e.MenuItem)
            {
                case "Dashboard":
                    Home home = new Home();
                    home.Show();
                    this.Close();
                    break;
                case "Violations":
                    break;
                case "SendViolions":
                    SendReport sendReport = new SendReport();
                    sendReport.Show();
                    this.Close();
                    break;
                case "Vehicles":
                    break;
                case "Users":
                    ManagerUser managerUser = new ManagerUser();
                        managerUser.Show();
                        this.Close();
                    break;
                case "Reports":
                    break;
                case "Notifications":
                    break;
                case "Settings":
                    break;
            }
        }
        private void SidebarMenu_LogoutClicked(object sender, EventArgs e)
        {
            var loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

        //private void btnDashboard_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Dashboard clicked!");
        //}

        //private void btnReports_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Reports clicked!");
        //}

        //private void btnViolations_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Violations clicked!");
        //}

        //private void btnVehicles_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Vehicles clicked!");
        //}

        //private void btnUsers_Click(object sender, RoutedEventArgs e)
        //{
        //    ManagerUser managerUser = new ManagerUser();
        //    managerUser.Show();
        //    this.Close();
        //}

        //private void btnNotifications_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Notifications clicked!");
        //}

        //private void btnSettings_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Settings clicked!");
        //}

        //private void btnLogout_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Logging out...");
        //    this.Close(); // Đóng ứng dụng hoặc chuyển về màn hình đăng nhập
        //}


    }
}
