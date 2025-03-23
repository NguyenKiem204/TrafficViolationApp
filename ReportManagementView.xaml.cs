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
    /// Interaction logic for ReportManagementView.xaml
    /// </summary>
    public partial class ReportManagementView : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Report> _reports;

        private ReportDAO reportDAO = new ReportDAO();


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Report> Reports
        {
            get { return _reports; }
            set
            {
                _reports = value;
                OnPropertyChanged("Reports");
            }
        }

        public ReportManagementView()
        {
            InitializeComponent();
            this.DataContext = this;

            reportDAO = new ReportDAO();
            LoadReports();
            sidebarMenu.SetActiveMenuItem("Reports");
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

        private void LoadReports()
        {
            try
            {
                var reportList = reportDAO.selectAll();
                Reports = new ObservableCollection<Report>(reportList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reports: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterReports()
        {
            string searchText = txtSearch.txtInput.Text.ToLower();
            string statusFilter = cmbStatus.SelectedIndex > 0 ? ((ComboBoxItem)cmbStatus.SelectedItem).Content.ToString() : null;

            var allReports = reportDAO.selectAll();
            var filteredReports = allReports;

            if (!string.IsNullOrEmpty(statusFilter))
            {
                filteredReports = filteredReports.Where(r => r.Status == statusFilter).ToList();
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredReports = filteredReports.Where(r =>
                    r.PlateNumber.ToLower().Contains(searchText) ||
                    r.ViolationType.ToLower().Contains(searchText) ||
                    r.Location.ToLower().Contains(searchText) ||
                    (r.Reporter != null && r.Reporter.FullName.ToLower().Contains(searchText))
                ).ToList();
            }

            Reports = new ObservableCollection<Report>(filteredReports);
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterReports();
        }

        private void cmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterReports();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadReports();
            txtSearch.txtInput.Text = string.Empty;
            cmbStatus.SelectedIndex = 0;
        }

        private void lvReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void btnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int reportId = Convert.ToInt32(btn.Tag);
                Report report = reportDAO.selectById(reportId);

                if (report != null)
                {
                    ReportDetailsView detailsView = new ReportDetailsView(report);
                    detailsView.Owner = this;
                    detailsView.ShowDialog();
                    LoadReports();
                }
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
                    // Already on Reports page
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
                    NotificationManagementView notificationView = new NotificationManagementView();
                    notificationView.Show();
                    this.Close();
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
