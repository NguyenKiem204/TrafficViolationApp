using System;
using System.Windows;
using System.Windows.Controls;

namespace TrafficViolationApp.view.UserControls
{
    public partial class StandardTextBox : UserControl
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(StandardTextBox), new PropertyMetadata(""));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public string Text
        {
            get { return txtInput.Text; }
            set { txtInput.Text = value; }
        }

        // Define a custom event for text changes
        public event EventHandler TextChanged;

        public StandardTextBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
                tbPlaceholder.Visibility = Visibility.Visible;
            else
                tbPlaceholder.Visibility = Visibility.Collapsed;

            // Raise the custom event
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            tbPlaceholder.Visibility = Visibility.Visible;
            txtInput.Focus();
        }
    }
}