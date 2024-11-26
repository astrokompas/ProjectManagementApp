using ProjectManagementApp.ViewModels;
using System.Windows;

namespace ProjectManagementApp.Windows
{
    public partial class EmployeeDialog : Window
    {
        private EmployeeDialogViewModel _viewModel;

        public EmployeeDialog()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) => this.DragMove();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _viewModel = DataContext as EmployeeDialogViewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _viewModel?.Cleanup();
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
    }
}