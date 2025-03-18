using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TrafficViolationApp.dao; // Import DAO namespace
using TrafficViolationApp.model; // Import Violation model namespace

namespace TrafficViolationApp
{
    public partial class Violations : Window
    {
        private int currentUserId;
        private ViolationDAO violationDAO;

        public Violations(int userId)
        {
            InitializeComponent();
            currentUserId = userId;  // Store the userId
            violationDAO = new ViolationDAO(); // Initialize ViolationDAO
            LoadViolations();
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
