using System.Configuration;
using System.Data;
using System.Windows;
using TrafficViolationApp.model;

namespace TrafficViolationApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                 UserSession.Instance.Reset();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thoát ứng dụng: {ex.Message}");
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Đã xảy ra lỗi không mong muốn: {e.Exception.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            Shutdown();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Đã xảy ra lỗi nghiêm trọng: {((Exception)e.ExceptionObject).Message}", "Lỗi nghiêm trọng", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

}
