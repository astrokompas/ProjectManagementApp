using System.Windows;

namespace ProjectManagementApp.Windows
{
    public partial class AddProjectDialog : Window
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }

        public AddProjectDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProjectName))
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