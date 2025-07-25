using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace CurrencyExchanger
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Transaction_report : Window
    {
        public Transaction_report()
        {
            InitializeComponent();
        }
        void OnLoad(object sender, RoutedEventArgs e)
        {
            if (Data.adminIsChecked == false)
            {
                UpdateBalance();
                UpdateFIO();
                AddToBalance.Content = "Пополнить баланс";
                AddToBalance.IsEnabled = true;
            }
            else
            {
                UpdateFIO();
                UpdateBalance();
                AddToBalance.Content = "У вас неограниченный баланс!";
                AddToBalance.IsEnabled = false;
            }
            
        }
        public void UpdateFIO()
        {
            fio_box.Text = Data.FIO;
        }
        public void UpdateBalance()
        {
            if (Data.adminIsChecked == false)
            {
                balance_box.Text = "Баланс: " + Data.Balance + " BYN";
                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                connection.Open();
                string command = $"UPDATE client set balance='{Data.Balance}' where login='{Data.Login}'";
                SqlCommand sqlCommand = new SqlCommand(command, connection);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                balance_box.Text = Data.post;
            }
            
        }

        private void AddToBalance_Click(object sender, RoutedEventArgs e)
        {
            Balance_add ba = new Balance_add();
            ba.ShowDialog();
            UpdateBalance();
        }

        private void Converter_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }

        private void Check_Currency_Rates_Click(object sender, RoutedEventArgs e)
        {
            Exchange_rates er = new Exchange_rates();
            er.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login();
            log.Show();
            Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (Data.adminIsChecked == false)
            {
                DataTable dt = new DataTable();
                string path = @"Data Source=BASS\SQLEXPRESS; Initial Catalog=CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                string command = $"select operation_type.name as 'Операция', operation.count as 'Сумма', currency.name as 'Валюта', currency.rate as 'Курс'," +
                    $" operation.transaction_date as 'Дата', operation_status.name as 'Статус' FROM operation_report join operation on operation.id=operation_report.operation_id" +
                    $" join operation_type on operation.operation_type_id=operation_type.id join operation_status on operation.status_id=operation_status.id join currency on operation.currency_id=currency.id where operation_report.client_id='{Data.client_id}'";
                connection.Open();
                SqlCommand newCommand = new SqlCommand(command, connection);
                newCommand.ExecuteNonQuery();
                SqlDataAdapter dataAdap = new SqlDataAdapter(newCommand);
                dt = new DataTable("View");
                dataAdap.Fill(dt);
                Report_grid.ItemsSource = dt.DefaultView;
                connection.Close();
            }
            else
            {
                DataTable dt = new DataTable();
                string path = @"Data Source=BASS\SQLEXPRESS; Initial Catalog=CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                string command = $"select client.login as 'Клиент', operation_type.name as 'Операция', operation.count as 'Сумма', currency.name as 'Валюта', currency.rate as 'Курс'," +
                    $" operation.transaction_date as 'Дата', operation_status.name as 'Статус' FROM operation_report join operation on operation.id=operation_report.operation_id" +
                    $" join operation_type on operation.operation_type_id=operation_type.id join operation_status on operation.status_id=operation_status.id join currency on operation.currency_id=currency.id join client on client.id=operation_report.client_id";
                connection.Open();
                SqlCommand newCommand = new SqlCommand(command, connection);
                newCommand.ExecuteNonQuery();
                SqlDataAdapter dataAdap = new SqlDataAdapter(newCommand);
                dt = new DataTable("View");
                dataAdap.Fill(dt);
                Report_grid.ItemsSource = dt.DefaultView;
                connection.Close();
            }
        }
    }
}
