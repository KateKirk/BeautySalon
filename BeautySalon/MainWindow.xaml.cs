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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeautySalon
{
    public partial class MainWindow : Window
    {
        private BeautySalonEntities context;
        public MainWindow()
        {
            InitializeComponent();
            context = new BeautySalonEntities();
            clientsDataGrid.ItemsSource = context.Client.ToList();
            genderComboBox.ItemsSource = context.Gender.ToList();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var client = new Client();
            context.Client.Add(client);
            ClientWindow clientWindow = new ClientWindow(context, client);
            clientWindow.ShowDialog();
            clientsDataGrid.ItemsSource = context.Client.ToList();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var row = (Client)clientsDataGrid.SelectedItem;
            if (row == null)
                MessageBox.Show("Выберите клиента для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                int idClient = row.ID;
                List<ClientService> clientServices = context.ClientService.ToList();
                clientServices = clientServices.Where(x => x.ClientID == idClient).ToList();
                if (clientServices.Count != 0)
                    MessageBox.Show("У данного клиента имеются посещения. Удаление запрещено.", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить данного клиента?", "Удаление клиента", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        context.Client.Remove(row);
                        context.SaveChanges();
                        clientsDataGrid.ItemsSource = context.Client.ToList();
                    }
                }
            }
        }

        private void fullNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchAndSortingTable();
        }

        private void phoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchAndSortingTable();
        }

        private void emailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchAndSortingTable();
        }
        private void SearchAndSortingTable()
        {
            var currentGender = (Gender)genderComboBox.SelectedItem;
            string searchFullName = fullNameTextBox.Text;
            string searchEmail = emailTextBox.Text;
            string searchPhone = phoneTextBox.Text;
            List<Client> clients = context.Client.ToList();
            if (genderComboBox.SelectedItem == null)
            {
                clients = clients.Where(x => x.LastName.ToLower().Contains(searchFullName.ToLower())).ToList();
                clients = clients.Where(x => x.Email.ToLower().Contains(searchEmail.ToLower())).ToList();
                clients = clients.Where(x => x.Phone.ToLower().Contains(searchPhone.ToLower())).ToList();
                clientsDataGrid.ItemsSource = clients;
            }
            else
            {
                clients = clients.Where(x => x.GenderCode == currentGender.Code).ToList();
                clients = clients.Where(x => x.Email.ToLower().Contains(searchEmail.ToLower())).ToList();
                clients = clients.Where(x => x.Phone.ToLower().Contains(searchPhone.ToLower())).ToList();
                clients = clients.Where(x => x.LastName.ToLower().Contains(searchFullName.ToLower())).ToList();
                clientsDataGrid.ItemsSource = clients;
            }
        }

        private void genderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchAndSortingTable();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            ClearValues();
            clientsDataGrid.ItemsSource = context.Client.ToList();
        }

        private void ClearValues()
        {
            fullNameTextBox.Text = string.Empty;
            emailTextBox.Text = string.Empty;
            phoneTextBox.Text = string.Empty;
            genderComboBox.SelectedItem = null;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            var client = editButton.DataContext as Client;
            ClientWindow clientWindow = new ClientWindow(context, client);
            clientWindow.ShowDialog();
        }
    }
}
