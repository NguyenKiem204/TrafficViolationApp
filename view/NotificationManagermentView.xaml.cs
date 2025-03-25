using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using TrafficViolationApp.dao;
using TrafficViolationApp.model;
using TrafficViolationApp.view.UserControls;

namespace TrafficViolationApp
{
    /// <summary>
    /// Interaction logic for NotificationManagementView.xaml
    /// </summary>
    public partial class NotificationManagementView : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Notification> _notifications;
        private NotificationDAO notificationDAO;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                OnPropertyChanged("Notifications");
            }
        }

        public NotificationManagementView()
        {
            InitializeComponent();
            this.DataContext = this;

            notificationDAO = new NotificationDAO();
            LoadNotifications();

            sidebarMenu.SetActiveMenuItem("Notifications");

            User currentUser = UserSession.Instance.User;
            if (currentUser != null)
            {
                sidebarMenu.SetUserInfo(currentUser.FullName, currentUser.Role, GetInitials(currentUser.FullName));
            }

            sidebarMenu.MenuItemSelected += SidebarMenu_MenuItemSelected;
            sidebarMenu.LogoutClicked += SidebarMenu_LogoutClicked;

            txtSearch.txtInput.TextChanged += TxtSearch_TextChanged;
        }

        private string GetInitials(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return "";

            var nameParts = fullName.Split(' ');
            if (nameParts.Length == 1) return nameParts[0].Substring(0, 1).ToUpper();

            return (nameParts[0].Substring(0, 1) + nameParts[nameParts.Length - 1].Substring(0, 1)).ToUpper();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadNotifications()
        {
            try
            {
                var notificationList = notificationDAO.selectAll();
                Notifications = new ObservableCollection<Notification>(notificationList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading notifications: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterNotifications()
        {
            string searchText = txtSearch.txtInput.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadNotifications();
                return;
            }

            var allNotifications = notificationDAO.selectAll();
            var filteredNotifications = allNotifications.Where(n =>
                (n.User != null && n.User.FullName.ToLower().Contains(searchText)) ||
                (n.PlateNumber != null && n.PlateNumber.ToLower().Contains(searchText)) ||
                n.Message.ToLower().Contains(searchText)
            ).ToList();

            Notifications = new ObservableCollection<Notification>(filteredNotifications);
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterNotifications();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadNotifications();
            txtSearch.txtInput.Text = string.Empty;
        }

        private void lvNotifications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvNotifications.SelectedItem is Notification selectedNotification && !selectedNotification.IsRead.GetValueOrDefault())
            {
                selectedNotification.IsRead = true;
                notificationDAO.update(selectedNotification);

                LoadNotifications();
            }
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
                case "Reports":
                    ReportManagementView reportView = new ReportManagementView();
                    reportView.Show();
                    this.Close();
                    break;
                case "Violations":
                    // Navigate to Violations
                    break;
                case "Vehicles":
                    // Navigate to Vehicles
                    break;
                case "Users":
                    ManagerUser managerUser = new ManagerUser();
                    managerUser.Show();
                    this.Close();
                    break;
                case "Notifications":
                    // Already on Notifications page
                    break;
                case "Settings":
                    // Navigate to Settings
                    break;
            }
        }


        private void SidebarMenu_LogoutClicked(object sender, EventArgs e)
        {
            var loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
    }
}
