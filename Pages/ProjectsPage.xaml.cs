using Microsoft.Extensions.DependencyInjection;
using ProjectManagementApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ProjectManagementApp.Pages
{
    public partial class ProjectsPage : Page
    {
        public ProjectsPage()
        {
            InitializeComponent();
            var viewModel = ((App)Application.Current).ServiceProvider
                .GetService<ProjectsViewModel>();
            DataContext = viewModel;
        }
    }
}