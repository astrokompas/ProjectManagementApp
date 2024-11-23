using System.Windows;

namespace ProjectManagementApp.Windows
{
    public partial class ErrorDialog : Window
    {
        public ErrorDialog(string message)
        {
            InitializeComponent();
            DataContext = message;

            // Allow dragging the window
            this.MouseLeftButtonDown += (s, e) => this.DragMove();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}