using FitnessHard.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for AddSubscriptionWindow.xaml
    /// </summary>
    public partial class AddSubscriptionWindow : Window
    {
        readonly FitnessHardContext ctx = new();
        SubscriptionClient subscriptionClient;

        public AddSubscriptionWindow(int? ClientId)
        {
            InitializeComponent();
            subscriptionClient = new SubscriptionClient()
            {
                Client = ctx.Clients.Find(ClientId)!,
                Employee = ctx.Employees.Find(Application.Current.Properties["CurUserId"])!,
                Status = true,
            };
            DataContext = subscriptionClient;
            SubscriptionTypeBox.ItemsSource = ctx.Subscriptions.ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ctx.SubscriptionClients.Add(subscriptionClient);
            ctx.SaveChanges();
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SubscriptionTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriceBox.Text = ((Subscription)SubscriptionTypeBox.SelectedItem).Price.ToString();
            DiscountBox.Text = "0";
            TotalBox.Text = PriceBox.Text;
        }
    }
}
