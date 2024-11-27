using ProjectManagementApp.ViewModels;
using System.Windows;

namespace ProjectManagementApp.Windows
{
    public partial class ContactDialog : Window
    {
        public ContactDialog()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (DataContext is ContactDialogViewModel viewModel)
            {
                viewModel.Cleanup();
            }
        }
    }
}