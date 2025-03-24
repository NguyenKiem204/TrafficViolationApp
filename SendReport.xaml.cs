using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using TrafficViolationApp.config;
using TrafficViolationApp.dao;
using TrafficViolationApp.model;
using TrafficViolationApp.view.UserControls;

namespace TrafficViolationApp
{
    public partial class SendReport : Window
    {
        private string imagePath = "";
        private string videoPath = "";
        private readonly CloudinaryService cloudinaryService;

        private int currentUserId;

        public SendReport()
        {
            InitializeComponent();
            try
            {
                cloudinaryService = new CloudinaryService();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo CloudinaryService: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            sidebarMenu.MenuItemSelected += SidebarMenu_MenuItemSelected;
            sidebarMenu.LogoutClicked += SidebarMenu_LogoutClicked;
            sidebarMenu.SetActiveMenuItem("SendViolions");
            dpViolationDate.SelectedDate = DateTime.Now;
            User user = UserSession.Instance.User;
            currentUserId = user.UserId;
            sidebarMenu.SetUserInfo(user.FullName, user.Role, "NK");
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
                    break;
                case "Vehicles":
                    break;
                case "Users":
                    ManagerUser managerUser = new ManagerUser();
                    managerUser.Show();
                    this.Close();
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

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    imagePath = openFileDialog.FileName;
                    txtImagePath.Text = Path.GetFileName(imagePath);

                    // Display image preview
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.EndInit();

                    imagePreview.Source = bitmap;
                    imagePreviewBorder.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearImagePreview();
                }
            }
        }

        private void btnClearImage_Click(object sender, RoutedEventArgs e)
        {
            ClearImagePreview();
        }

        private void ClearImagePreview()
        {
            imagePreview.Source = null;
            imagePreviewBorder.Visibility = Visibility.Collapsed;
            txtImagePath.Text = "No image selected";
            imagePath = "";
        }

        private void btnUploadVideo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Video files (*.mp4, *.avi, *.mov)|*.mp4;*.avi;*.mov",
                Title = "Select a Video"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    videoPath = openFileDialog.FileName;
                    txtVideoPath.Text = Path.GetFileName(videoPath);

                    // Set up video preview
                    videoPreview.Source = new Uri(videoPath);
                    videoPreviewBorder.Visibility = Visibility.Visible;

                    // Load the first frame but don't play automatically
                    videoPreview.Position = TimeSpan.FromMilliseconds(1);
                    videoPreview.Pause();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading video: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearVideoPreview();
                }
            }
        }

        private void btnPlayVideo_Click(object sender, RoutedEventArgs e)
        {
            if (videoPreview.Source != null)
            {
                videoPreview.Play();
            }
        }

        private void btnPauseVideo_Click(object sender, RoutedEventArgs e)
        {
            if (videoPreview.Source != null)
            {
                videoPreview.Pause();
            }
        }

        private void btnStopVideo_Click(object sender, RoutedEventArgs e)
        {
            if (videoPreview.Source != null)
            {
                videoPreview.Stop();
                videoPreview.Position = TimeSpan.FromMilliseconds(1);
            }
        }

        private void btnClearVideo_Click(object sender, RoutedEventArgs e)
        {
            ClearVideoPreview();
        }

        private void ClearVideoPreview()
        {
            if (videoPreview.Source != null)
            {
                videoPreview.Stop();
                videoPreview.Source = null;
            }
            videoPreviewBorder.Visibility = Visibility.Collapsed;
            txtVideoPath.Text = "No video selected";
            videoPath = "";
        }

        private void btnSubmitReport_Click(object sender, RoutedEventArgs e)
        {
            if (cmbViolationType.SelectedItem == null)
            {
                MessageBox.Show("Please select a violation type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPlateNumber.Text))
            {
                MessageBox.Show("Please enter the vehicle plate number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter the location where the violation occurred.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpViolationDate.SelectedDate == null)
            {
                MessageBox.Show("Please select the date of the violation.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please provide a description of the violation.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string imageUrl = "";
            string videoUrl = "";

            try
            {
                btnSubmitReport.IsEnabled = false;
                btnSubmitReport.Content = "Submitting...";
                Mouse.OverrideCursor = Cursors.Wait;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    imageUrl = cloudinaryService.UploadImage(imagePath);
                }
                if (!string.IsNullOrEmpty(videoPath))
                {
                    videoUrl = cloudinaryService.UploadVideo(videoPath);
                }

                // Create and save the report with cloud URLs
                Report report = new Report()
                {
                    ViolationType = GetSelectedViolationType(),
                    PlateNumber = txtPlateNumber.Text.Trim(),
                    Location = txtLocation.Text.Trim(),
                    ReportDate = dpViolationDate.SelectedDate.Value,
                    Description = txtDescription.Text.Trim(),
                    ImageUrl = imageUrl,
                    VideoUrl = videoUrl,
                    ReporterId = currentUserId
                };

                ReportDAO reportDAO = new ReportDAO();
                int success = reportDAO.insert(report);

                if (success == 1)
                {
                    MessageBox.Show("Report submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to submit report. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Reset cursor and enable button
                Mouse.OverrideCursor = null;
                btnSubmitReport.IsEnabled = true;
                btnSubmitReport.Content = "Submit Report";
            }
        }

        private string GetSelectedViolationType()
        {
            if (cmbViolationType.SelectedItem != null)
            {
                return ((ComboBoxItem)cmbViolationType.SelectedItem).Content.ToString();
            }
            return "";
        }

        private void ClearForm()
        {
            cmbViolationType.SelectedIndex = -1;
            txtPlateNumber.Text = "";
            txtLocation.Text = "";
            dpViolationDate.SelectedDate = DateTime.Now;
            txtDescription.Text = "";

            // Clear media previews
            ClearImagePreview();
            ClearVideoPreview();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (videoPreview.Source != null)
            {
                videoPreview.Stop();
                videoPreview.Source = null;
            }

            base.OnClosing(e);
        }
    }
}