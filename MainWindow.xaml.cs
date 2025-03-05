using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrafficViolationApp.dao;
using TrafficViolationApp.model;

namespace TrafficViolationApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserDAO userDAO = new UserDAO();
                User user = new User("Nguyễn Đăng Nguyên", "nuyen0508@gmail.com", "123", "TrafficPolice", "0336780144", "Hà Nội");

                int result = userDAO.insert(user);

                if (result > 0)
                    MessageBox.Show("Thêm user thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Thêm user thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}