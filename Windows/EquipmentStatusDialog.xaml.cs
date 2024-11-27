using System.Windows;
using ProjectManagementApp.ViewModels;
using ProjectManagementApp.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagementApp.Windows
{
    public partial class EquipmentStatusDialog : Window
    {
        private readonly EquipmentStatusDialogViewModel _viewModel;
        public string SelectedStatus => _viewModel.SelectedStatus;
        public int? SelectedProjectId => _viewModel.SelectedProject?.Id;

        public EquipmentStatusDialog(EquipmentItemViewModel equipment)
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) => this.DragMove();
            var projectRepository = ((App)Application.Current).ServiceProvider
                .GetService<IProjectRepository>();
            _viewModel = new EquipmentStatusDialogViewModel(equipment, projectRepository);
            DataContext = _viewModel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CanExecuteSave())
            {
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}