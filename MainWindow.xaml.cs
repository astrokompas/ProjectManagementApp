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

        public enum NavigationPages
        {
            Projekty,
            Ludzie,
            Sprzet,
            Baza,
            Kontakty,
            Raport
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
            if (sender is RadioButton radioButton && radioButton.Tag != null)
            {
                if (Enum.TryParse<NavigationPages>(radioButton.Tag.ToString(), out NavigationPages page))
                {
                    switch (page)
                    {
                        case NavigationPages.Projekty:
                            MainFrame.Navigate(new ProjectsPage());
                            break;
                        case NavigationPages.Ludzie:
                            // MainFrame.Navigate(new TasksPage());
                            break;
                        case NavigationPages.Sprzet:
                            // MainFrame.Navigate(new TeamPage());
                            break;
                        case NavigationPages.Baza:
                            // MainFrame.Navigate(new TeamPage());
                            break;
                        case NavigationPages.Kontakty:
                            MainFrame.Navigate(new ContactsPage());
                            break;
                        case NavigationPages.Raport:
                            MainFrame.Navigate(new ReportsPage());
                            break;
                    }
                }
            }
        }
    }
}