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
    /// Interaction logic for ManagerUser.xaml
    /// </summary>
    public partial class ManagerUser : Window, INotifyPropertyChanged
    {
        private ObservableCollection<User> _users;
        private UserDAO userDAO;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged("Users");
            }
        }

        public ManagerUser()
        {
            InitializeComponent();
            this.DataContext = this;

            userDAO = new UserDAO();
            sidebarMenu.SetActiveMenuItem("Users");
            User user = UserSession.Instance.User;
            sidebarMenu.SetUserInfo(user.FullName, user.Role, user.GetInitials());
            LoadUsers();

            sidebarMenu.MenuItemSelected += SidebarMenu_MenuItemSelected;
            sidebarMenu.LogoutClicked += SidebarMenu_LogoutClicked;

            txtSearch.txtInput.TextChanged += TxtSearch_TextChanged;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadUsers()
        {
            try
            {
                var userList = userDAO.selectAll();
                Users = new ObservableCollection<User>(userList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterUsers(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadUsers();
                return;
            }

            searchText = searchText.ToLower();
            var filteredUsers = userDAO.selectAll().Where(u =>
                u.FullName.ToLower().Contains(searchText) ||
                u.Email.ToLower().Contains(searchText) ||
                u.Phone.ToLower().Contains(searchText) ||
                (u.Address != null && u.Address.ToLower().Contains(searchText)) ||
                u.Role.ToLower().Contains(searchText) ||
                u.UserId.ToString().Contains(searchText)
            ).ToList();

            Users = new ObservableCollection<User>(filteredUsers);
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

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterUsers(txtSearch.txtInput.Text);
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            var userDetailsWindow = new UserDetailsWindow();
            userDetailsWindow.Owner = this;
            userDetailsWindow.ShowDialog();

            if (userDetailsWindow.IsSuccess)
            {
                LoadUsers();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int userId = Convert.ToInt32(button.Tag);

            var userToEdit = userDAO.selectById(userId);
            if (userToEdit != null)
            { 
                var userDetailsWindow = new UserDetailsWindow(userToEdit);
                userDetailsWindow.Owner = this;
                userDetailsWindow.ShowDialog();

                if (userDetailsWindow.IsSuccess)
                {
                    LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("User not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int userId = Convert.ToInt32(button.Tag);
            var result = MessageBox.Show($"Are you sure you want to delete user with ID {userId}?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var userToDelete = userDAO.selectById(userId);
                    if (userToDelete != null)
                    {
                        int deleteResult = userDAO.delete(userToDelete);
                        if (deleteResult > 0)
                        {
                            MessageBox.Show("User deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadUsers();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete user", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void lvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}