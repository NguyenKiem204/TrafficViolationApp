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
            string email = txtEmail.Text;
            string password = txtPass.Password;
            if (userDAO.checkLogin(email, password))
            {
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
            else
            {
                txtEmail.Text = email;
                txtPass.Password = password;
                txtError.Text = "Email or Password wrong";
            }
        }


    }
}
