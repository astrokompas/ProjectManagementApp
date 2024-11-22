using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectManagementApp.Pages
{
    public partial class ContactsPage : UserControl
    {
        public ContactsPage()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).ServiceProvider.GetService<ContactsViewModel>();
        }
    }
}
