using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementApp.ViewModels;
using System.Windows;

namespace ProjectManagementApp.Pages
{
    public partial class HQPage : Page
    {
        public HQPage()
        {
            InitializeComponent();
            var viewModel = ((App)Application.Current).ServiceProvider
                .GetService<HQViewModel>();
            DataContext = viewModel;
        }
    }
}