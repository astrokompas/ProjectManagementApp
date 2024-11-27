using ProjectManagementApp.Models;
using System.Windows;

namespace ProjectManagementApp.Windows
{
    public partial class EditProjectDialog : Window
    {
        public Project Project { get; private set; }

        public EditProjectDialog(Project project)
        {
            InitializeComponent();
            Project = project;
            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Project.Name))
            {
                MessageBox.Show("Nazwa projektu jest wymagana.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}