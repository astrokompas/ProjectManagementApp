using ProjectManagementApp.ViewModels;
using System.Windows;

namespace ProjectManagementApp.Windows
{
    public partial class EquipmentDialog : Window
    {
        private EquipmentDialogViewModel _viewModel;

        public EquipmentDialog()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) => this.DragMove();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _viewModel = DataContext as EquipmentDialogViewModel;
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