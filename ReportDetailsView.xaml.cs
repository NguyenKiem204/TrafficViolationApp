using System;
using System.Collections.Generic;
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

namespace TrafficViolationApp
{
    /// <summary>
    /// Interaction logic for ReportDetailsView.xaml
    /// </summary>
    public partial class ReportDetailsView : Window, INotifyPropertyChanged
    {
        private Report _report;
        private Vehicle _vehicle;
        private BitmapImage _imageSource;
        private string _processedByName;
        private ReportDAO reportDAO;
        private VehicleDAO vehicleDAO;
        private ViolationDAO violationDAO;
        private NotificationDAO notificationDAO;

        public event PropertyChangedEventHandler PropertyChanged;

        public Report Report
        {
            get { return _report; }
            set
            {
                _report = value;
                OnPropertyChanged("Report");
            }
        }

        public Vehicle Vehicle
        {
            get { return _vehicle; }
            set
            {
                _vehicle = value;
                OnPropertyChanged("Vehicle");
            }
        }

        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public string ProcessedByName
        {
            get { return _processedByName; }
            set
            {
                _processedByName = value;
                OnPropertyChanged("ProcessedByName");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ReportDetailsView(Report report)
        {
            InitializeComponent();
            this.DataContext = this;

            reportDAO = new ReportDAO();
            vehicleDAO = new VehicleDAO();
            violationDAO = new ViolationDAO();
            notificationDAO = new NotificationDAO();

            Report = report;
            LoadVehicleInfo();
            LoadImage();
            UpdateUIBasedOnStatus();
        }
        private void LoadVehicleInfo()
        {
            try
            {
                Vehicle = vehicleDAO.selectByPlateNumber(Report.PlateNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading vehicle information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadImage()
        {
            try
            {
                if (!string.IsNullOrEmpty(Report.ImageUrl))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Report.ImageUrl);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ImageSource = bitmap;
                }
                else
                {
                    ImageSource = new BitmapImage(new Uri("/images/no-image.png", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ImageSource = new BitmapImage(new Uri("/images/no-image.png", UriKind.Relative));
            }
        }

        private void UpdateUIBasedOnStatus()
        {
            if (Report.Status != "Pending")
            {
                txtFineAmount.IsEnabled = false;
                txtComments.IsEnabled = false;
                btnApprove.IsEnabled = false;
                btnReject.IsEnabled = false;

                if (Report.Status == "Rejected")
                {
                    pnlRejectionReason.Visibility = Visibility.Visible;
                    txtRejectionReason.IsEnabled = false;
                }

                if (Report.ProcessedByNavigation != null)
                {
                    ProcessedByName = Report.ProcessedByNavigation.FullName;
                }
                else
                {
                    ProcessedByName = "Unknown";
                }
            }
            else
            {
                ProcessedByName = "Not processed yet";
            }
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFineAmount.Text) || !decimal.TryParse(txtFineAmount.Text, out decimal fineAmount))
            {
                MessageBox.Show("Please enter a valid fine amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Report.Status = "Approved";
                Report.ProcessedBy = UserSession.Instance.User.UserId;

                int result = reportDAO.update(Report);
                if (result > 0)
                {
                    Violation violation = new Violation
                    {
                        ReportId = Report.ReportId,
                        PlateNumber = Report.PlateNumber,
                        ViolatorId = Vehicle?.OwnerId,
                        FineAmount = fineAmount,
                        FineDate = DateTime.Now,
                        PaidStatus = false
                    };

                    int violationId = violationDAO.insert(violation);
                    if (Vehicle?.OwnerId != null)
                    {
                        Notification notification = new Notification
                        {
                            UserId = Vehicle.OwnerId,
                            PlateNumber = Report.PlateNumber,
                            Message = $"Your vehicle with plate number {Report.PlateNumber} was reported for {Report.ViolationType}. " +
                                     $"Fine amount: ${fineAmount:N2}. " +
                                     $"Comments: {txtComments.Text}",
                            SentDate = DateTime.Now,
                            IsRead = false
                        };

                        int notificationId = notificationDAO.insert(notification);
                        if (notificationId > 0)
                        {
                            MessageBox.Show("Report approved and notification sent successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Report approved but failed to send notification.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Report approved but vehicle owner not found for notification.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Failed to approve report.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            pnlRejectionReason.Visibility = Visibility.Visible;
            if (string.IsNullOrWhiteSpace(txtRejectionReason.Text))
            {
                MessageBox.Show("Please provide a reason for rejection.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Report.Status = "Rejected";
                Report.ProcessedBy = UserSession.Instance.User.UserId;

                int result = reportDAO.update(Report);
                if (result > 0)
                {
                    MessageBox.Show("Report rejected successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to reject report.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rejecting report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
