using ProjectManagementApp.Repositories;
using ProjectManagementApp.ViewModels;
using System;
using System.Collections.Generic;
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

namespace ProjectManagementApp.Windows
{
    public partial class AssignResourcesDialog : Window
    {
        public AssignResourcesDialog(ResourceType resourceType, int projectId,
            IProjectAssignmentService assignmentService,
            IEmployeeRepository employeeRepository,
            IEquipmentRepository equipmentRepository)
        {
            InitializeComponent();
            DataContext = new AssignResourcesViewModel(
                resourceType,
                projectId,
                assignmentService,
                employeeRepository,
                equipmentRepository);

            ((AssignResourcesViewModel)DataContext).RequestClose += (result) =>
            {
                DialogResult = result;
                Close();
            };
        }

        private void ResourceTile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element &&
                element.DataContext is ResourceItemViewModel resource)
            {
                var viewModel = (AssignResourcesViewModel)DataContext;
                viewModel.ToggleSelectionCommand.Execute(resource);
            }
        }
    }
}
