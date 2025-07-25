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
    public partial class Balance_add : Window
    {
        public Balance_add()
        {
            InitializeComponent();
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
           
        }
        public void getOperation_id()
        {
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            DataTable dt = new DataTable();
            connection.Open();
            string command = $"SELECT id FROM operation WHERE currency_id LIKE 2";
            SqlCommand sqlCommand = new SqlCommand(command, connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            dt = new DataTable("Rate");

            dataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                var dr = sqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    Data.operation_id = int.Parse(dr[0].ToString());
                }
                dr.Close();
                sqlCommand.ExecuteNonQuery();
                connection.Close();

            }
        }

        public void operationAdd()
        {
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            connection.Open();
            string command = $"insert into operation(operation_type_id, status_id, currency_id, transaction_date, count) values (2, 1, 2, '{Data.Date}', '{float.Parse(countBox.Text.ToString())}')";
            SqlCommand sqlCommand = new SqlCommand(command, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        public void operation_reportAdd()
        {
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            connection.Open();
            string command = $"insert into operation_report(admin_id, client_id, operation_id) values(1, '{Data.client_id}', '{Data.operation_id}')";
            SqlCommand sqlCommand = new SqlCommand(command, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        private void countBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }

        private void AddBalance_Click(object sender, RoutedEventArgs e)
        {
            if (countBox.Text == "")
            {
                MessageBox.Show("Введите значения", "Ошибка");
            }
            else
            {
                Data.Date = DateTime.Now;
                operationAdd();
                getOperation_id();
                operation_reportAdd();
                Data.Balance += float.Parse(countBox.Text.ToString());
                Close();
            }
            
        }
    }
}
