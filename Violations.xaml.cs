using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TrafficViolationApp.dao; 
using TrafficViolationApp.view.UserControls;

namespace TrafficViolationApp
{
    public partial class Violations : Window, INotifyPropertyChanged
    {
        private int currentUserId;
        private ViolationDAO violationDAO;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Violations(int userId)
        {
            InitializeComponent();
            currentUserId = userId;  // Store the userId
            violationDAO = new ViolationDAO(); // Initialize ViolationDAO
            LoadViolations();
        }

        private void SidebarMenu_MenuItemSelected(object sender, MenuItemSelectedEventArgs e)
        {
            // Navigate to different pages based on menu selection
            switch (e.MenuItem)
            {
                case "Dashboard":
                    Home home = new Home();
                    home.Show();
                    this.Close();
                    break;
                case "Violations":
                    // Navigate to Violations
                    break;
                case "Vehicles":
                    // Navigate to Vehicles
                    break;
                case "Users":
                    // Already on Users page
                    break;
                case "Reports":
                    // Navigate to Reports
                    break;
                case "Notifications":
                    // Navigate to Notifications
                    break;
                case "Settings":
                    // Navigate to Settings
                    break;
            }
        }

        private void LoadViolations()
        {
            try
            {
                // Get violations for the current user
                var violations = violationDAO.selectByUserId(currentUserId);

                // Bind the violations to the ListView
                lvViolations.ItemsSource = violations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading violations: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRespond_Click(object sender, RoutedEventArgs e)
        {
            // Add your code for what happens when the button is clicked
            MessageBox.Show("Button clicked!");
        }

        private void lvViolations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Add your code for what happens when the selection changes
            var selectedItem = lvViolations.SelectedItem;
            if (selectedItem != null)
            {
                // Do something with the selected item
            }
        }

    }
}
