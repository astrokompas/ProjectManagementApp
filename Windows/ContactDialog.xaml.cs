using System.Windows;
using System.Windows.Input;

namespace ProjectManagementApp.Windows
{
    public partial class ContactDialog : Window
    {
        public ContactDialog()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) => this.DragMove();
        }
    }
}