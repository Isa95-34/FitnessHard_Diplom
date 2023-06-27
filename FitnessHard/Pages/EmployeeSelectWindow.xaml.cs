using FitnessHard.Models;
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
using System.Windows.Shapes;

namespace FitnessHard.Pages
{
    /// <summary>
    /// Interaction logic for EmployeeSelectWindow.xaml
    /// </summary>
    public partial class EmployeeSelectWindow : Window
    {
        private readonly FitnessHardContext ctx = new();
        public EmployeeSelectWindow(JournalService js)
        {
            InitializeComponent();
            TrainerBox.ItemsSource = ctx.Employees.ToList();
            TrainerBox.SelectedIndex = 0;
            DataContext = js;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            return;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            return;
        }
    }
}
