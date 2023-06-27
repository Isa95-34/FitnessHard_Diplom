using FitnessHard.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FitnessHard.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        readonly FitnessHardContext _context = new();
        readonly Employee curUser;
        private JournalService NoRegService = new();

        public MainPage(int? roleId)
        {
            InitializeComponent();
            _context.Subscriptions.Load();
            _context.ServicesSubscriptions.Load();
            _context.JournalServices.Load();
            _context.Services.Load();
            _context.Employees.Load();
            _context.Clients.Load();
            SubscriptionGrid.ItemsSource = _context.SubscriptionClients.ToList();
            SubscriptionGrid.SelectedIndex = 0;
            curUser = _context.Employees.Find(Application.Current.Properties["CurUserId"])!;
            ClientBox.ItemsSource = _context.SubscriptionClients.ToList();
            ServiceBox.ItemsSource = _context.Services.ToList();
           
            UpdateNoRegService();

            NoRegServicePanel.DataContext = NoRegService;
            if (roleId == 1)
            {
                AdminDel.Visibility = Visibility.Visible;
                menuStackPanel.Visibility = Visibility.Visible;
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubscriptionGrid.SelectedItem == null)
            {
                MessageBox.Show("Вы не выбрали абонемент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы действительно хотите удалить выбранный абонемент?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SubscriptionClient subscriptionClient = (SubscriptionClient)SubscriptionGrid.SelectedItem;
                _context.SubscriptionClients.Remove(subscriptionClient);
                _context.SaveChanges();
                SubscriptionGrid.ItemsSource = _context.SubscriptionClients.ToList();
                MessageBox.Show("Абонемент успешно удален.");
            }
        }
        private void UpdateNoRegService()
        {
            NoRegService = new()
            {
                Service = (Service)ServiceBox.SelectedItem, // Используется выбранный тренер из ComboBox
                Employee = curUser,
                DateUse = DateTime.Now,
            };
        }

        private void SubscriptionGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubscriptionGrid.SelectedItem != null)
            {
                SubscriptionClient? subClient = SubscriptionGrid.SelectedItem as SubscriptionClient;

                if (subClient != null)
                {
                    Subscription sub = subClient.Subscription;

                    List<ServiceForGrid> list = new List<ServiceForGrid>();

                    foreach (ServicesSubscription servicesSubscription in sub.ServicesSubscriptions)
                    {
                        list.Add(new ServiceForGrid()
                        {
                            Title = servicesSubscription.Service.Title,
                            Count = servicesSubscription.Count - subClient.JournalServices.Count(js => js.Service == servicesSubscription.Service),
                            ServiceId = servicesSubscription.Service.Id,
                        });
                    }

                    ServiceGrid.ItemsSource = list;
                    ServiceLogGrid.ItemsSource = subClient.JournalServices.ToList();
                    SubscriptionInfoBlock.Text = $"Клиент: {subClient.Client.Name}. Дата действия абонемента: {subClient.DateStart} - {subClient.DateEnd} {(subClient.DateEnd < DateTime.Today ? "ПРОСРОЧЕН!" : "")}";
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceGrid.SelectedItem == null)
            {
                MessageBox.Show("Вы не выбрали услугу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ServiceForGrid serviceForGrid = (ServiceGrid.SelectedItem as ServiceForGrid)!;
            Service service = _context.Services.Find(serviceForGrid.ServiceId)!;
            if (serviceForGrid.Count <= 0)
            {
                MessageBox.Show("Услуги данного типа в абонементе закончились", "Операция прервана", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            JournalService journalService = new()
            {
                Cost = service.Price,
                DateUse = DateTime.Now,
                Employee = curUser,
                SubscriptionClient = (SubscriptionClient)SubscriptionGrid.SelectedItem,
                Service = service,
                Trainer = (Employee)ServiceBox.SelectedItem, // Используется выбранный тренер из ComboBox
            };
            EmployeeSelectWindow employeeSelectWindow = new(journalService);
            if (employeeSelectWindow.ShowDialog() == false)
                return;
            _context.Entry(journalService).State = journalService.Id == 0 ? EntityState.Added : EntityState.Modified;
            _context.SaveChanges();
            ServiceLogGrid.ItemsSource = ((SubscriptionClient)SubscriptionGrid.SelectedItem).JournalServices.ToList();
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "")
            {
                SubscriptionGrid.ItemsSource = _context.SubscriptionClients.ToList();
                return;
            }
            SubscriptionGrid.ItemsSource = _context.SubscriptionClients.Where(sc => sc.Id.ToString().Equals(SearchBox.Text)).ToList();
            SubscriptionGrid.SelectedIndex = 0;
        }   


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceBox.SelectedIndex == -1 || ClientBox.SelectedIndex == -1)
                return;

            if (ServiceBox.SelectedIndex == -1 || ClientBox.SelectedIndex == -1)
                return;
            _context.Entry(NoRegService).State = EntityState.Added;
            _context.SaveChanges();

            MessageBox.Show("Запись успешна.");
        }

        private void ServiceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NoRegService.Cost = ((Service)ServiceBox.SelectedItem).Price;
        }

        private void ClientMenuClick(object sender, RoutedEventArgs e)
        {
            ClientMenuWindow clientMenuWindow = new();
            clientMenuWindow.ShowDialog();
        }

        private void SubscriptionMenuClick(object sender, RoutedEventArgs e)
        {
            SubscriptionMenuWindow subscriptionMenuWindow = new();
            subscriptionMenuWindow.ShowDialog();
        }


        private void ServiceLogGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServiceLogGrid.SelectedItem != null)
            {
                JournalService selectedJournalService = (JournalService)ServiceLogGrid.SelectedItem;

                // Обновление данных в NoRegServicePanel
                NoRegServicePanel.DataContext = selectedJournalService;

                // Обновление ComboBox для выбора тренера
                ServiceBox.SelectedItem = selectedJournalService.Trainer;
            }
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceBox.SelectedIndex == -1 || ClientBox.SelectedIndex == -1 || ServiceBox.SelectedIndex == -1)
                return;

            SubscriptionClient selectedClient = (SubscriptionClient)ClientBox.SelectedItem;
            Service selectedService = (Service)ServiceBox.SelectedItem;


            JournalService journalService = new JournalService
            {
                Cost = selectedService.Price,
                DateUse = DateTime.Now,
                Employee = curUser,
                Trainer = curUser,
                SubscriptionClient = selectedClient,
                Service = selectedService   
            };

            _context.JournalServices.Add(journalService);
            _context.SaveChanges();

            ServiceLogGrid.ItemsSource = selectedClient.JournalServices.ToList();
        }





        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                ServiceLogGrid.Measure(pageSize);
                ServiceLogGrid.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(ServiceLogGrid, Title);
            }
        }
    }
}