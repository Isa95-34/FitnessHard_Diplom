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
    /// Interaction logic for SubscriptionMenuWindow.xaml
    /// </summary>
    public partial class SubscriptionMenuWindow : Window
    {
        readonly FitnessHardContext ctx = new();
        SubscriptionClient? subscriptionClient;

        public SubscriptionMenuWindow()
        {
            InitializeComponent();
            ctx.Subscriptions.Load();
            ctx.Clients.Load();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionClient? sc = ctx.SubscriptionClients.SingleOrDefault(sc => sc.Id.ToString() == SearchBox.Text);
            if (sc == null)
            {
                MessageBox.Show("Абонемент не найден");
                return;
            }
            subscriptionClient = sc;
            ActiveText.Text = sc.Status ? "Активен" : "Заморожен";
            DataContext = subscriptionClient;
        }

        private void ChangeEndDate_Click(object sender, RoutedEventArgs e)
        {
            if (subscriptionClient == null)
                return;
            ctx.Entry(subscriptionClient).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            ctx.SaveChanges();
        }

        private void FreezeButton_Click(object sender, RoutedEventArgs e)
        {
            if (subscriptionClient != null)
                subscriptionClient.Status = false;
        }

        private void ActivateButton_Click(object sender, RoutedEventArgs e)
        {
            if (subscriptionClient != null)
                subscriptionClient.Status = true;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
