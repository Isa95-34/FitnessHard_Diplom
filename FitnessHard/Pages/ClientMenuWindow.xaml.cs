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
    /// Interaction logic for ClientMenuWindow.xaml
    /// </summary>
    public partial class ClientMenuWindow : Window
    {
        readonly FitnessHardContext ctx = new();
        Client CurrentClient = new();

        public ClientMenuWindow()
        {
            InitializeComponent();
            ctx.Clients.Load();
            Update();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedIndex < 0)
                return;
            Client selectedClient = (Client)ClientsGrid.SelectedItem;
            ctx.Clients.Remove(selectedClient);
            ctx.SaveChanges();
            Update();

            MessageBox.Show("Клиент успешно удален!", "Удаление клиента", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e) // Добавление
        {
            SaveClientButton.IsEnabled = true;
            DobEditBox.IsEnabled = true;
            FullNameEditBox.IsEnabled = true;
            PhoneNumberEditBox.IsEnabled = true;            
            CurrentClient = new Client();
            ClientPanel.DataContext = CurrentClient;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e) // Удаление
        {
            if (ClientsGrid.SelectedIndex < 0)
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Client selectedClient = (Client)ClientsGrid.SelectedItem;
            ctx.Clients.Remove(selectedClient);
            ctx.SaveChanges();
            Update();
            MessageBox.Show("Клиент успешно удален!", "Удаление клиента", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditClientButton_Click(object sender, RoutedEventArgs e) // Редактирование
        {
            SaveClientButton.IsEnabled = true;
            DobEditBox.IsEnabled = true;
            FullNameEditBox.IsEnabled = true;
            PhoneNumberEditBox.IsEnabled = true;
        }

        private void AddSubscription_Click(object sender, RoutedEventArgs e) // Оформление абонемента
        {
            if (ClientsGrid.SelectedItem is Client selectedClient)
            {
                int clientId = selectedClient.Id;
                AddSubscriptionWindow addSubscriptionWindow = new AddSubscriptionWindow(clientId);
                addSubscriptionWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveClientButton_Click(object sender, RoutedEventArgs e) // Сохранение
        {
            SaveClientButton.IsEnabled = false;
            DobEditBox.IsEnabled = false;
            FullNameEditBox.IsEnabled = false;
            PhoneNumberEditBox.IsEnabled = false;

            if (string.IsNullOrEmpty(CurrentClient.Name))
            {
                MessageBox.Show("Пожалуйста, введите имя клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ctx.Entry(CurrentClient).State = CurrentClient.Id == 0 ? EntityState.Added : EntityState.Modified;
            ctx.SaveChanges();
            Update();

            string message = CurrentClient.Id == 0 ? "Клиент успешно добавлен!" : "Информация о клиенте успешно обновлена!";
            MessageBox.Show(message, "Сохранение клиента", MessageBoxButton.OK, MessageBoxImage.Information);

            
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            List<Client> list = ctx.Clients.ToList();
            if (FullNameBox.Text != "")
                list = list.Where(c => c.Name.Contains(FullNameBox.Text)).ToList();
            if (PhoneNumberBox.Text != "")
                list = list.Where(c => c.Phone.Contains(PhoneNumberBox.Text)).ToList();
            ClientsGrid.ItemsSource = list;
        }

        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsGrid.SelectedIndex < 0)
                return;
            EditButton.IsEnabled = true;
            RemoveButton.IsEnabled = true;
            EditClientButton.IsEnabled = true;
            CurrentClient = (ClientsGrid.SelectedItem as Client)!;
            ClientPanel.DataContext = CurrentClient;
        }

        private void Update()
        {
            ClientsGrid.ItemsSource = ctx.Clients.ToList();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
