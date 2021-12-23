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

namespace BeautySalon
{
    /// <summary>
    /// Логика взаимодействия для ClientsVisitsWindiow.xaml
    /// </summary>
    public partial class ClientsVisitsWindiow : Window
    {
        BeautySalonEntities context;
        Client client;
        public ClientsVisitsWindiow(BeautySalonEntities context, int clientId)
        {
            InitializeComponent();
            this.context = context;
           // this.client = ;
            visitsDataGrid.ItemsSource = context.ClientService.ToList();
        }
    }
}
