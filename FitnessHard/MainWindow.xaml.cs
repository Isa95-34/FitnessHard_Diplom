using FitnessHard.Pages;
using System.Windows;

namespace FitnessHard
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Pages.AuthPage AuthPage = new Pages.AuthPage();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = AuthPage;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MinHeight = 500;
            MinWidth = 815;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = AuthPage;
            LoggedInAsText.Visibility = Visibility.Hidden;
            LogoutButton.Visibility = Visibility.Hidden;
        }
    }
}
