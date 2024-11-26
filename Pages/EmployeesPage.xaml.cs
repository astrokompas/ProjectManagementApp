using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementApp.ViewModels;

namespace ProjectManagementApp.Pages
{
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();

            var viewModel = ((App)Application.Current).ServiceProvider
                .GetService<EmployeesViewModel>();

            DataContext = viewModel;
        }
    }
}