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
using TrafficViolationApp.dao;
using TrafficViolationApp.model;

namespace TrafficViolationApp
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        private User currentUser;
        private UserDAO userDAO;
        private bool isEditMode;

        public bool IsSuccess { get; private set; }

        public UserDetailsWindow(User user = null)
        {
            InitializeComponent();
            userDAO = new UserDAO();
            isEditMode = user != null;

            if (isEditMode)
            {
                currentUser = user;
                txtTitle.Text = "Edit User";
                LoadUserData();
                txtPasswordNotice.Visibility = Visibility.Visible;
                //MessageBox.Show(currentUser.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                currentUser = new User();
                txtTitle.Text = "Add New User";
            }
        }

        private void LoadUserData()
        {
            txtFullName.Text = currentUser.FullName;
            txtEmail.Text = currentUser.Email;
            txtPhone.Text = currentUser.Phone;
            txtAddress.Text = currentUser.Address;
            txtPassword.Password = "";
            foreach (ComboBoxItem item in cmbRole.Items)
            {
                if (item.Content.ToString() == currentUser.Role)
                {
                    cmbRole.SelectedItem = item;
                    break;
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter the full name", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!isEditMode && string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please enter a password", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter a phone number", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                currentUser.FullName = txtFullName.Text;
                currentUser.Email = txtEmail.Text;
                currentUser.Phone = txtPhone.Text;
                currentUser.Address = txtAddress.Text;
                currentUser.Password = "";
                currentUser.Role = (cmbRole.SelectedItem as ComboBoxItem).Content.ToString();
                if (!isEditMode || !string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    currentUser.Password = txtPassword.Password;
                }

                int result;
                if (isEditMode)
                {
                    result = userDAO.update(currentUser);
                    MessageBox.Show("SHOW UPDATE DATA: " + currentUser.ToString(), "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    result = userDAO.insert(currentUser);
                }

                if (result > 0)
                {
                    MessageBox.Show(isEditMode ? "User updated successfully" : "User added successfully",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    IsSuccess = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(isEditMode ? "Failed to update user" : "Failed to add user",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}