using System.Windows;
using System.Windows.Controls;
using TrafficViolationApp.config;
using TrafficViolationApp.dao;
using TrafficViolationApp.model;

namespace TrafficViolationApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            if (UserCredentialManager.TryGetCredentials(out string email, out string password))
            {
                txtEmail.Text = email;
                txtPass.Password = password;
                chkRememberMe.IsChecked = true;
            }
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtError.Text = "";
        }

        private void txtPass_GotFocus(object sender, RoutedEventArgs e)
        {
            txtError.Text = "";
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtError.Text = "";
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserDAO userDAO = new UserDAO();
            string email = txtEmail.Text.Trim();
            string password = txtPass.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Email hoặc mật khẩu không được để trống!";
                return;
            }

            User? user = userDAO.CheckLogin(email, password);
            if (user != null)
            {
                if (chkRememberMe.IsChecked == true)
                {
                    UserCredentialManager.SaveCredentials(email, password);
                }
                else
                {
                    UserCredentialManager.ClearCredentials();
                }
                UserSession.Instance.User = user;
                Home home = new Home();
                home.Show();
                this.Hide();
            }
            else
            {
                txtEmail.Text = email;
                txtPass.Password = "";
                txtError.Text = "Email hoặc mật khẩu không đúng!";
            }
        }

        private void txtForgotPassword_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void txtRegister_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}