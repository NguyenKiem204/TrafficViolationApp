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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
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
                txtError.Text = "Email or Password cannot be empty!";
                return;
            }
            User? user = userDAO.CheckLogin(email, password);
            if (user != null)
            {
                UserSession.Instance.User = user;
                Home home = new Home();
                home.Show();
                this.Hide();
            }
            else
            {
                txtEmail.Text = email;
                txtPass.Password = "";
                txtError.Text = "Invalid email or password!";
            }
        }



    }
}
