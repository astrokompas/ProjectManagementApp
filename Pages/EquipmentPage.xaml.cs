using Microsoft.Extensions.DependencyInjection;
using ProjectManagementApp.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace ProjectManagementApp.Pages
{
    public partial class EquipmentPage : Page
    {
        public EquipmentPage()
        {
            InitializeComponent();
            var viewModel = ((App)Application.Current).ServiceProvider
                .GetService<EquipmentViewModel>();
            DataContext = viewModel;
        }
    }
}
