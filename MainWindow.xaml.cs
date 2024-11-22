using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjectManagementApp.Pages;

namespace ProjectManagementApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ProjectsPage());
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                MaximizeButton_Click(sender, e);
            }
            else
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NavButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                switch (radioButton.Tag.ToString())
                {
                    case "Projekty":
                        // MainFrame.Navigate(new ProjectsPage());
                        break;
                    case "Ludzie":
                        // MainFrame.Navigate(new TasksPage());
                        break;
                    case "Sprzęt":
                        // MainFrame.Navigate(new TeamPage());
                        break;
                    case "Baza":
                        // MainFrame.Navigate(new TeamPage());
                        break;
                    case "Kontakty":
                        // MainFrame.Navigate(new TeamPage());
                        break;
                    case "Raport":
                        MainFrame.Navigate(new ReportsPage());
                        break;
                }
            }
        }
    }
}