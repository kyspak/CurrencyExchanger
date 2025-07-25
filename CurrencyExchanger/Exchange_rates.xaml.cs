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
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CurrencyExchanger
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Exchange_rates : Window
    {
        public Regex regex = new Regex("^[0-9]+");
        public Exchange_rates()
        {
            InitializeComponent();
        }
        void OnLoad(object sender, RoutedEventArgs e)
        {
            
            UpdateBalance();
            UpdateFIO();
            curShow();
            if (Data.adminIsChecked == false)
            {
                AddToBalance.Content = "Пополнить баланс";
                Currency_rates.MaxHeight = 450;
                AddToBalance.IsEnabled = true;
                stackAdd.Visibility=Visibility.Hidden;
            }
            else
            {
                Currency_rates.MaxHeight = 215;
                stackAdd.Visibility = Visibility.Visible;
                AddToBalance.Content = "У вас неограниченный баланс!";
                AddToBalance.IsEnabled = false;
            }
        }

        public void curShow()
        {
            DataTable dt = new DataTable();
            string path = @"Data Source=BASS\SQLEXPRESS; Initial Catalog=CurrencyExchanger; Integrated Security=True";

            SqlConnection connection = new SqlConnection(path);
            string command = "SELECT id as 'id', rate as 'Курс', name as 'Валюта' FROM currency";

            connection.Open();

            SqlCommand newCommand = new SqlCommand(command, connection);
            newCommand.ExecuteNonQuery();

            SqlDataAdapter dataAdap = new SqlDataAdapter(newCommand);
            dt = new DataTable("Discounts");
            dataAdap.Fill(dt);
            Currency_rates.ItemsSource = dt.DefaultView;

            connection.Close();
        }
        private void AddToBalance_Click(object sender, RoutedEventArgs e)
        {
            Balance_add ba = new Balance_add();
            ba.ShowDialog();
            UpdateBalance();
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

        private void Converter_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            Transaction_report tp = new Transaction_report();
            tp.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login();
            log.Show();
            Close();
        }

        private void Currency_name_box_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Currency_Add_Click(object sender, RoutedEventArgs e)
        {
            if(Currency_name_box.Text=="" || Currency_rate_box.Text == "")
            {
                MessageBox.Show("Введите значения", "Ошибка");
            }
            else
            {
                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                connection.Open();
                string command = $"insert into currency(rate, name) values ({Currency_rate_box.Text}, '{Currency_name_box.Text}')";
                SqlCommand sqlCommand = new SqlCommand(command, connection);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                curShow();
            }
            
        }

        private void Currency_rate_box_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 .".IndexOf(e.Text) < 0;
        }

        private void curEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Currency_name_box.Text == "" || Currency_rate_box.Text == "" || ID_box.Text == "")
            {
                MessageBox.Show("Введите значения", "Ошибка");
            }
            else
            {

                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                connection.Open();
                string command = $"update currency set rate={Currency_rate_box.Text}, name='{Currency_name_box.Text}' where id={int.Parse(ID_box.Text)}";
                SqlCommand sqlCommand = new SqlCommand(command, connection);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                curShow();
            }
        }

        private void ID_box_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void curDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ID_box.Text == "")
            {
                MessageBox.Show("Введите значения", "Ошибка");
            }
            else
            {

                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                connection.Open();
                string command = $"delete currency where id={int.Parse(ID_box.Text)}";
                SqlCommand sqlCommand = new SqlCommand(command, connection);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                curShow();
            }
        }

        private void curFind_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            string path = @"Data Source=BASS\SQLEXPRESS; Initial Catalog=CurrencyExchanger; Integrated Security=True";

            SqlConnection connection = new SqlConnection(path);
            string command = $"SELECT id as 'id', rate as 'Курс', name as 'Валюта' FROM currency where name='{Currency_name_box.Text}'";

            connection.Open();

            SqlCommand newCommand = new SqlCommand(command, connection);
            newCommand.ExecuteNonQuery();

            SqlDataAdapter dataAdap = new SqlDataAdapter(newCommand);
            dt = new DataTable("Discounts");
            dataAdap.Fill(dt);
            Currency_rates.ItemsSource = dt.DefaultView;

            connection.Close();
        }
    }
}
