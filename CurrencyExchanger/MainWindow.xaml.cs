using System;
using System.Windows;

using System.Windows.Input;

using System.Data;
using System.Data.SqlClient;


namespace CurrencyExchanger
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string FIO;

        public MainWindow()
        {
            InitializeComponent();

        }
        public void getFIO()
        {
            if (Data.adminIsChecked == false)
            {
                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                DataTable dt = new DataTable();
                connection.Open();
                string command = $"SELECT FIO FROM client WHERE login LIKE '{Data.Login}'";
                SqlCommand sqlCommand = new SqlCommand(command, connection);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dt = new DataTable("Client");

                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    var dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        fio_box.Text = dr[0].ToString();
                        Data.FIO = dr[0].ToString();
                    }

                    dr.Close();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    MessageBox.Show(Data.Login, "да...");
                }
            }
            else
            {
                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                DataTable dt = new DataTable();
                connection.Open();
                string command = $"SELECT FIO FROM admin WHERE login LIKE '{Data.Login}'";
                SqlCommand sqlCommand = new SqlCommand(command, connection);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dt = new DataTable("Admin");

                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    var dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        fio_box.Text = dr[0].ToString();
                        Data.FIO = dr[0].ToString();
                    }

                    dr.Close();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    MessageBox.Show(Data.Login, "да...");
                }
            }
           
        }

        public void fill_Combo()
        {
            comboFrom.SelectedIndex = 0;
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            string command = $"SELECT name FROM currency";
            SqlCommand sqlCommand = new SqlCommand(command, connection);
            connection.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            dt = new DataTable("Portfolio");

            dataAdapter.Fill(dt);
            var dr = sqlCommand.ExecuteReader();
           
            while (dr.Read())
            {
                comboFrom.Items.Add(dr[0]);
               
            }
            
            connection.Close();
        }

        public void fill_Combo2()
        {
            comboTo.SelectedIndex = 0;
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            string command = $"SELECT name FROM currency";
            SqlCommand sqlCommand = new SqlCommand(command, connection);
            connection.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            dt = new DataTable("Portfolio");

            dataAdapter.Fill(dt);
            var dr = sqlCommand.ExecuteReader();

            while (dr.Read())
            {
                comboTo.Items.Add(dr[0]);

            }

            connection.Close();
        }
        public void getBalance()
        {
            if (Data.adminIsChecked == false)
            {
                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                DataTable dt = new DataTable();
                connection.Open();
                string command = $"SELECT balance FROM client WHERE login LIKE '{Data.Login}'";
                SqlCommand sqlCommand = new SqlCommand(command, connection);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dt = new DataTable("Client");

                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    var dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        Data.Balance = float.Parse(dr[0].ToString());
                    }
                    balance_box.Text = "Баланс: " + Data.Balance.ToString() + " BYN";
                    dr.Close();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    MessageBox.Show(Data.Balance.ToString(), "да...");
                }
            }
            else
            {
                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                DataTable dt = new DataTable();
                connection.Open();
                string command = $"SELECT post.name FROM post join admin on post.admin_id=admin.id WHERE admin.login LIKE '{Data.Login}'";
                SqlCommand sqlCommand = new SqlCommand(command, connection);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dt = new DataTable("Admin");

                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    var dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        balance_box.Text = dr[0].ToString();
                    }
                    Data.post = balance_box.Text;
                    dr.Close();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    MessageBox.Show(Data.Balance.ToString(), "да...");
                }
            }
            
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            if (Data.adminIsChecked == false)
            {
                getFIO();
                getBalance();
                fill_Combo();
                fill_Combo2();
                getClient_id();
                AddToBalance.Content = "Пополнить баланс";
                AddToBalance.IsEnabled = true;
            }
            else
            {
                getFIO();
                AddToBalance.Content = "У вас неограниченный баланс!";
                AddToBalance.IsEnabled = false;
                fill_Combo();
                fill_Combo2();
                getBalance();
            }
        }

        private void countBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }

        public void getToRate()
        {
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            DataTable dt = new DataTable();
            connection.Open();
            string command = $"SELECT rate FROM currency WHERE name LIKE '{comboTo.SelectedItem.ToString()}'";
            SqlCommand sqlCommand = new SqlCommand(command, connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            dt = new DataTable("Rate");

            dataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                var dr = sqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    Data.ToRate = float.Parse(dr[0].ToString());
                }
                dr.Close();
                sqlCommand.ExecuteNonQuery();
                connection.Close();

            }
        }

        public void getFromRate()
        {
            
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            DataTable dt = new DataTable();
            connection.Open();
            string command = $"SELECT rate FROM currency WHERE name LIKE '{comboFrom.SelectedItem.ToString()}'";
            SqlCommand sqlCommand = new SqlCommand(command, connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            dt = new DataTable("Rate");

            dataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                var dr = sqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    Data.FromRate = float.Parse(dr[0].ToString());
                }
                dr.Close();
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (countBox.Text == "")
            {
                MessageBox.Show("Введите данные", "Ошибка");
            }
            else
            {
                if (Data.adminIsChecked == false)
                {
                    getFromRate();
                    getToRate();
                    float res = float.Parse(countBox.Text) * Data.FromRate / Data.ToRate;
                    if (res * Data.ToRate > Data.Balance)
                    {
                        MessageBox.Show("Недостаточно средств", "Внимание");
                    }
                    else
                    {
                        Data.Balance -= res * Data.ToRate;
                        Data.count = float.Parse(countBox.Text);
                        MessageBox.Show(res + " " + comboTo.SelectedItem.ToString(), "Переведено успешно");
                        UpdateBalance();
                    }
                    operationRecord();
                }
                else
                {
                    Data.Balance = 10000;
                    getFromRate();
                    getToRate();
                    float res = float.Parse(countBox.Text) * Data.FromRate / Data.ToRate;
                    if (res * Data.ToRate > Data.Balance)
                    {
                        MessageBox.Show("Недостаточно средств", "Внимание");
                    }
                    else
                    {
                        Data.Balance -= res * Data.ToRate;
                        Data.count = float.Parse(countBox.Text);
                        MessageBox.Show(res + " " + comboTo.SelectedItem.ToString(), "Переведено успешно");
                    }
                }
            }
        }
        
        public void UpdateBalance()
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
        private void AddToBalance_Click(object sender, RoutedEventArgs e)
        {
            
            Balance_add ba = new Balance_add();
            ba.ShowDialog();
            UpdateBalance();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login();
            log.Show();
            Close();
        }

  

        public void operationAdd()
        {
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            connection.Open();
            string command = $"insert into operation(operation_type_id, status_id, currency_id, transaction_date, count) values (1, 1, '{Data.currency_id}', '{Data.Date}', '{Data.count}')";
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
        public void operationRecord()
        {
            
            Data.Date = DateTime.Now;
            getClient_id();
            getCurrency_id();
            operationAdd();
            getOperation_id(); 
            operation_reportAdd();
        }

        public void getClient_id()
        {
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            DataTable dt = new DataTable();
            connection.Open();
            string command = $"SELECT id FROM client WHERE login LIKE '{Data.Login}'";
            SqlCommand sqlCommand = new SqlCommand(command, connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            dt = new DataTable("Client");

            dataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                var dr = sqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    Data.client_id = int.Parse(dr[0].ToString());
                }
                dr.Close();
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                MessageBox.Show(Data.Balance.ToString(), "да...");
            }
        }

        public void getCount()
        {
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            DataTable dt = new DataTable();
            connection.Open();
            string command = $"SELECT count FROM operation WHERE id LIKE '{Data.operation_id}'";
            SqlCommand sqlCommand = new SqlCommand(command, connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            dt = new DataTable("Rate");

            dataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                var dr = sqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    Data.count = int.Parse(dr[0].ToString());
                }
                dr.Close();
                sqlCommand.ExecuteNonQuery();
                connection.Close();

            }
        }
        public void getCurrency_id()
        {
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            DataTable dt = new DataTable();
            connection.Open();
            string command = $"SELECT id FROM currency WHERE name LIKE '{comboFrom.SelectedItem.ToString()}'";
            SqlCommand sqlCommand = new SqlCommand(command, connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            dt = new DataTable("Rate");

            dataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                var dr = sqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    Data.currency_id = int.Parse(dr[0].ToString());
                }
                dr.Close();
                sqlCommand.ExecuteNonQuery();
                connection.Close();

            }
        }
        
        public void getOperation_id()
        {
            string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
            SqlConnection connection = new SqlConnection(path);
            DataTable dt = new DataTable();
            connection.Open();
            string command = $"SELECT id FROM operation WHERE currency_id LIKE '{Data.currency_id}'";
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
        private void Check_Currency_Rates_Click(object sender, RoutedEventArgs e)
        {
            Exchange_rates er = new Exchange_rates();
            er.Show();
            Close();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            Transaction_report tp = new Transaction_report();
            tp.Show();
            Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }
    }
}
