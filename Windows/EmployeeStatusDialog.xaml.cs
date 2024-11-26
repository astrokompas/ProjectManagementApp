using System;
using System.Windows;
using ProjectManagementApp.ViewModels;
using ProjectManagementApp.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagementApp.Windows
{
    public partial class EmployeeStatusDialog : Window
    {
        private readonly EmployeeStatusDialogViewModel _viewModel;

        public string SelectedStatus => _viewModel.SelectedStatus;
        public DateTime? VacationStart => _viewModel.VacationStart;
        public DateTime? VacationEnd => _viewModel.VacationEnd;
        public int? SelectedProjectId => _viewModel.SelectedProject?.Id;

        public EmployeeStatusDialog(EmployeeViewModel employee)
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) => this.DragMove();

            var projectRepository = ((App)Application.Current).ServiceProvider
                .GetService<IProjectRepository>();

            _viewModel = new EmployeeStatusDialogViewModel(employee, projectRepository);
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