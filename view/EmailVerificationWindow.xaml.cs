using System;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrafficViolationApp.dao;
using TrafficViolationApp.service;

namespace TrafficViolationApp.view
{
    public partial class EmailVerificationWindow : Window
    {
        private readonly UserDAO userDAO;
        private readonly EmailService emailService;
        private string userEmail;
        private DispatcherTimer otpTimer;
        private TimeSpan remainingTime;
        private const int OTP_VALID_MINUTES = 10;

        public EmailVerificationWindow(string email)
        {
            InitializeComponent();
            userDAO = new UserDAO();
            emailService = CreateEmailService();

            userEmail = email;
            txtUserEmail.Text = email;

            // Initialize UI state
            SetVisibilityState(VerificationState.Pending);

            // Setup OTP fields
            SetupOtpTimer(OTP_VALID_MINUTES);
            SetupOtpKeyboardNavigation();
            txtOtp1.Focus();

            // Send initial verification email
            SendInitialVerificationEmail(email);
        }

        private async void SendInitialVerificationEmail(string email)
        {
            try
            {
                var user = userDAO.selectByEmail(email);
                if (user != null && !string.IsNullOrEmpty(user.OtpCode))
                {
                    string emailBody = GenerateOtpEmailTemplate(user.FullName, user.OtpCode, OTP_VALID_MINUTES);
                    await emailService.SendEmailAsync(email, "Mã xác thực OTP - Hệ thống quản lý vi phạm giao thông", emailBody);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi email: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private enum VerificationState
        {
            Pending,
            Success,
            Failed
        }

        private void SetVisibilityState(VerificationState state)
        {
            VerificationPending.Visibility = state == VerificationState.Pending ? Visibility.Visible : Visibility.Collapsed;
            VerificationSuccess.Visibility = state == VerificationState.Success ? Visibility.Visible : Visibility.Collapsed;
            VerificationFailed.Visibility = state == VerificationState.Failed ? Visibility.Visible : Visibility.Collapsed;
        }

        private EmailService CreateEmailService()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSettings = config.AppSettings.Settings;

            string smtpServer = appSettings["SmtpServer"]?.Value ?? "smtp.gmail.com";
            int smtpPort = int.Parse(appSettings["SmtpPort"]?.Value ?? "587");
            string smtpUsername = appSettings["SmtpUsername"]?.Value ?? "your-email@gmail.com";
            string smtpPassword = appSettings["SmtpPassword"]?.Value ?? "your-app-password";
            string senderEmail = appSettings["SenderEmail"]?.Value ?? "your-email@gmail.com";
            string senderName = appSettings["SenderName"]?.Value ?? "Traffic Violation System";

            return new EmailService(smtpServer, smtpPort, smtpUsername, smtpPassword, senderEmail, senderName);
        }

        #region OTP Input Handling

        private void SetupOtpKeyboardNavigation()
        {
            // Apply handlers to all OTP textboxes
            TextBox[] otpTextBoxes = { txtOtp1, txtOtp2, txtOtp3, txtOtp4, txtOtp5, txtOtp6 };
            foreach (var textBox in otpTextBoxes)
            {
                textBox.PreviewKeyDown += OtpTextBox_PreviewKeyDown;
                textBox.GotFocus += OtpTextBox_GotFocus;
                textBox.PreviewTextInput += TextBox_PreviewTextInput;
                textBox.TextChanged += OtpTextBox_TextChanged;
            }
        }

        private void OtpTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private void OtpTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox currentTextBox)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        MoveToPreviousTextBox(currentTextBox);
                        e.Handled = true;
                        break;

                    case Key.Right:
                        MoveToNextTextBox(currentTextBox);
                        e.Handled = true;
                        break;

                    case Key.Back:
                        if (string.IsNullOrEmpty(currentTextBox.Text))
                        {
                            MoveToPreviousTextBox(currentTextBox);
                            e.Handled = true;
                        }
                        break;

                    case Key.Delete:
                        if (!string.IsNullOrEmpty(currentTextBox.Text))
                        {
                            currentTextBox.Text = string.Empty;
                            e.Handled = true;
                        }
                        break;

                    case Key.V:
                        if (Keyboard.Modifiers == ModifierKeys.Control)
                        {
                            HandleOtpPaste();
                            e.Handled = true;
                        }
                        break;
                }
            }
        }

        private void MoveToPreviousTextBox(TextBox currentTextBox)
        {
            if (currentTextBox == txtOtp2) txtOtp1.Focus();
            else if (currentTextBox == txtOtp3) txtOtp2.Focus();
            else if (currentTextBox == txtOtp4) txtOtp3.Focus();
            else if (currentTextBox == txtOtp5) txtOtp4.Focus();
            else if (currentTextBox == txtOtp6) txtOtp5.Focus();
        }

        private void MoveToNextTextBox(TextBox currentTextBox)
        {
            if (currentTextBox == txtOtp1) txtOtp2.Focus();
            else if (currentTextBox == txtOtp2) txtOtp3.Focus();
            else if (currentTextBox == txtOtp3) txtOtp4.Focus();
            else if (currentTextBox == txtOtp4) txtOtp5.Focus();
            else if (currentTextBox == txtOtp5) txtOtp6.Focus();
        }

        private void HandleOtpPaste()
        {
            if (Clipboard.ContainsText())
            {
                string clipText = Clipboard.GetText().Trim();
                if (Regex.IsMatch(clipText, @"^\d{6}$"))
                {
                    txtOtp1.Text = clipText[0].ToString();
                    txtOtp2.Text = clipText[1].ToString();
                    txtOtp3.Text = clipText[2].ToString();
                    txtOtp4.Text = clipText[3].ToString();
                    txtOtp5.Text = clipText[4].ToString();
                    txtOtp6.Text = clipText[5].ToString();
                    btnVerifyOtp.Focus();
                }
            }
        }

        private void OtpTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;
            if (currentTextBox != null && currentTextBox.Text.Length == 1)
            {
                if (currentTextBox == txtOtp1) txtOtp2.Focus();
                else if (currentTextBox == txtOtp2) txtOtp3.Focus();
                else if (currentTextBox == txtOtp3) txtOtp4.Focus();
                else if (currentTextBox == txtOtp4) txtOtp5.Focus();
                else if (currentTextBox == txtOtp5) txtOtp6.Focus();
                else if (currentTextBox == txtOtp6 && IsAllOtpFieldsFilled())
                {
                    btnVerifyOtp.Focus();
                }
            }
            else if (currentTextBox != null && currentTextBox.Text.Length == 0)
            {
                if (currentTextBox == txtOtp2) txtOtp1.Focus();
                else if (currentTextBox == txtOtp3) txtOtp2.Focus();
                else if (currentTextBox == txtOtp4) txtOtp3.Focus();
                else if (currentTextBox == txtOtp5) txtOtp4.Focus();
                else if (currentTextBox == txtOtp6) txtOtp5.Focus();
            }
        }

        private bool IsAllOtpFieldsFilled()
        {
            return !string.IsNullOrEmpty(txtOtp1.Text) &&
                   !string.IsNullOrEmpty(txtOtp2.Text) &&
                   !string.IsNullOrEmpty(txtOtp3.Text) &&
                   !string.IsNullOrEmpty(txtOtp4.Text) &&
                   !string.IsNullOrEmpty(txtOtp5.Text) &&
                   !string.IsNullOrEmpty(txtOtp6.Text);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private string GetEnteredOtp()
        {
            return txtOtp1.Text + txtOtp2.Text + txtOtp3.Text + txtOtp4.Text + txtOtp5.Text + txtOtp6.Text;
        }

        private void ClearOtpFields()
        {
            txtOtp1.Text = txtOtp2.Text = txtOtp3.Text = txtOtp4.Text = txtOtp5.Text = txtOtp6.Text = string.Empty;
        }

        #endregion

        #region OTP Timer

        private void SetupOtpTimer(int minutes)
        {
            remainingTime = TimeSpan.FromMinutes(minutes);
            UpdateTimerDisplay();

            otpTimer?.Stop();
            otpTimer = new DispatcherTimer();
            otpTimer.Interval = TimeSpan.FromSeconds(1);
            otpTimer.Tick += OtpTimer_Tick;
            otpTimer.Start();

            // Reset timer appearance
            txtTimer.Foreground = System.Windows.Media.Brushes.Black;
            btnVerifyOtp.IsEnabled = true;
        }

        private void UpdateTimerDisplay()
        {
            txtTimer.Text = $"Mã OTP sẽ hết hạn sau: {remainingTime.Minutes:00}:{remainingTime.Seconds:00}";
        }

        private void OtpTimer_Tick(object sender, EventArgs e)
        {
            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
            UpdateTimerDisplay();

            if (remainingTime.TotalSeconds <= 0)
            {
                otpTimer.Stop();
                txtTimer.Text = "Mã OTP đã hết hạn. Vui lòng yêu cầu mã mới.";
                txtTimer.Foreground = System.Windows.Media.Brushes.Red;
                btnVerifyOtp.IsEnabled = false;
            }
        }

        #endregion

        #region Email Operations

        private async Task<bool> SendOtpEmailAsync(string email)
        {
            bool resent = userDAO.ResendVerificationEmail(email);

            if (resent)
            {
                var user = userDAO.selectByEmail(email);
                if (user != null && !string.IsNullOrEmpty(user.OtpCode))
                {
                    string emailBody = GenerateOtpEmailTemplate(user.FullName, user.OtpCode, OTP_VALID_MINUTES);
                    await emailService.SendEmailAsync(email, "Mã xác thực OTP - Hệ thống quản lý vi phạm giao thông", emailBody);
                    return true;
                }
            }

            return false;
        }

        private async void btnResendEmail_Click(object sender, RoutedEventArgs e)
        {
            await HandleEmailSending(btnResendEmail, resendEmailProgress, userEmail);
        }

        private async void btnRequestNewCode_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmailForResend.Text.Trim();

            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ email hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool success = await HandleEmailSending(btnRequestNewCode, requestNewCodeProgress, email);

            if (success)
            {
                userEmail = email;
                txtUserEmail.Text = email;
                SetVisibilityState(VerificationState.Pending);
            }
        }

        private async Task<bool> HandleEmailSending(Button button, ProgressBar progressBar, string email)
        {
            string originalButtonContent = button.Content.ToString();
            try
            {
                // UI state: sending
                button.IsEnabled = false;
                button.Content = "Đang gửi...";
                progressBar.Visibility = Visibility.Visible;

                // Send email
                bool result = await SendOtpEmailAsync(email);

                if (result)
                {
                    // Reset timer and OTP fields
                    SetupOtpTimer(OTP_VALID_MINUTES);
                    ClearOtpFields();
                    txtOtp1.Focus();

                    MessageBox.Show("Mã OTP đã được gửi. Vui lòng kiểm tra email của bạn.",
                        "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    return true;
                }
                else
                {
                    throw new Exception("Không tìm thấy tài khoản với email này hoặc tài khoản đã được xác thực.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                // Reset UI state
                button.IsEnabled = true;
                button.Content = originalButtonContent;
                progressBar.Visibility = Visibility.Collapsed;
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        private string GenerateOtpEmailTemplate(string fullName, string otpCode, int validMinutes)
        {
            return $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        margin: 0;
                        padding: 0;
                        background-color: #f4f4f4;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        padding: 20px;
                        background-color: #fff;
                        border-radius: 8px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    .header {{
                        background-color: #6C63FF;
                        color: white;
                        padding: 15px;
                        text-align: center;
                        border-top-left-radius: 8px;
                        border-top-right-radius: 8px;
                        margin-bottom: 20px;
                    }}
                    .otp-code {{
                        font-size: 36px;
                        font-weight: bold;
                        letter-spacing: 5px;
                        text-align: center;
                        color: #6C63FF;
                        margin: 20px 0;
                        padding: 10px;
                        background-color: #f0f0ff;
                        border-radius: 5px;
                    }}
                    .footer {{
                        margin-top: 20px;
                        text-align: center;
                        font-size: 12px;
                        color: #888;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Xác thực Email</h2>
                    </div>
                    <div class='content'>
                        <p>Kính gửi <b>{fullName}</b>,</p>
                        <p>Cảm ơn bạn đã đăng ký tài khoản tại Hệ thống quản lý vi phạm giao thông. Để hoàn tất quá trình đăng ký, vui lòng sử dụng mã OTP dưới đây:</p>
                        
                        <div class='otp-code'>{otpCode}</div>
                        
                        <p>Mã OTP này có hiệu lực trong vòng <b>{validMinutes} phút</b> kể từ khi email này được gửi.</p>
                        
                        <p><b>Lưu ý:</b></p>
                        <ul>
                            <li>Không chia sẻ mã OTP này cho bất kỳ ai.</li>
                            <li>Nếu bạn không thực hiện đăng ký này, vui lòng bỏ qua email này.</li>
                        </ul>
                        
                        <p>Nếu bạn gặp vấn đề trong quá trình xác thực, vui lòng liên hệ với chúng tôi qua địa chỉ email hỗ trợ.</p>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} Hệ thống quản lý vi phạm giao thông. Tất cả các quyền được bảo lưu.</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        #endregion

        #region Button Events

        private async void btnVerifyOtp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string otp = GetEnteredOtp();

                if (string.IsNullOrWhiteSpace(otp) || otp.Length != 6)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ mã OTP 6 chữ số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                btnVerifyOtp.IsEnabled = false;
                verifyingProgress.Visibility = Visibility.Visible;

                await Task.Delay(1000);

                bool verified = userDAO.VerifyEmail(userEmail, otp);

                if (verified)
                {
                    otpTimer?.Stop();
                    SetVisibilityState(VerificationState.Success);
                }
                else
                {
                    SetVisibilityState(VerificationState.Failed);
                    txtEmailForResend.Text = userEmail;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnVerifyOtp.IsEnabled = true;
                verifyingProgress.Visibility = Visibility.Collapsed;
            }
        }

        private void btnGoToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLogin();
        }

        private void txtLoginLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

        private void NavigateToLogin()
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

        #endregion
    }
}