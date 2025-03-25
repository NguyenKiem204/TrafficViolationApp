using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrafficViolationApp.dao;
using TrafficViolationApp.model;

namespace TrafficViolationApp.view
{
    public partial class RegisterWindow : Window
    {
        private readonly UserDAO userDAO;

        public RegisterWindow()
        {
            InitializeComponent();
            userDAO = new UserDAO();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs())
                {
                    btnRegister.IsEnabled = false;
                    btnRegister.Content = "Đang xử lý...";

                    User newUser = new User
                    {
                        FullName = txtFullName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Password = txtPassword.Password.Trim(),
                        Role = ((ComboBoxItem)cboRole.SelectedItem).Content.ToString(),
                        Phone = txtPhone.Text.Trim(),
                        Address = txtAddress.Text.Trim()
                    };

                    bool emailExists = userDAO.emailExists(newUser.Email);
                    if (emailExists)
                    {
                        ShowErrorMessage("Email này đã được sử dụng. Vui lòng chọn email khác.");
                        txtEmail.Focus();
                        btnRegister.IsEnabled = true;
                        btnRegister.Content = "Đăng ký";
                        return;
                    }

                    int result = userDAO.insert(newUser);

                    if (result > 0)
                    {
                        ShowSuccessMessage("Đăng ký tài khoản thành công!\nVui lòng xác thực email của bạn để hoàn tất đăng ký.");
                        EmailVerificationWindow verifyWindow = new EmailVerificationWindow(newUser.Email);
                        verifyWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        ShowErrorMessage("Đăng ký tài khoản thất bại. Vui lòng thử lại sau.");
                        btnRegister.IsEnabled = true;
                        btnRegister.Content = "Đăng ký";
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi đăng ký: {ex.Message}");
                btnRegister.IsEnabled = true;
                btnRegister.Content = "Đăng ký";
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowErrorMessage("Vui lòng nhập họ và tên.");
                txtFullName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                ShowErrorMessage("Vui lòng nhập địa chỉ email hợp lệ.");
                txtEmail.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Password) || txtPassword.Password.Length < 6)
            {
                ShowErrorMessage("Mật khẩu phải có ít nhất 6 ký tự.");
                txtPassword.Focus();
                return false;
            }
            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                ShowErrorMessage("Xác nhận mật khẩu không khớp.");
                txtConfirmPassword.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPhone.Text) || !IsValidPhoneNumber(txtPhone.Text))
            {
                ShowErrorMessage("Vui lòng nhập số điện thoại hợp lệ.");
                txtPhone.Focus();
                return false;
            }

            return true;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPhoneNumber(string phone)
        {
            string pattern = @"^[\d\+\-\(\)]{10,15}$";
            return Regex.IsMatch(phone, pattern);
        }

        private void txtLoginLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
    }
}