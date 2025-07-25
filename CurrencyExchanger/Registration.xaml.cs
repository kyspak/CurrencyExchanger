using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
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
using System.Text.RegularExpressions;
namespace CurrencyExchanger
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public string login;
        public string FIO;
        public string age;
        public Regex regex = new Regex("^[0-9]+");
        public Registration()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "" || ageBox.Text=="" || fioBox.Text == "")
            {
                MessageBox.Show("Введите значения!", "Ошибка");
            }
            else
            {
                string password = ps.Password.ToString();
                if (password == "")
                {
                    MessageBox.Show("Введите значения!", "Ошибка");
                }
                else
                {
                    string password_second = ps2.Password.ToString();
                    if (password == password_second)
                    {
                        string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                        SqlConnection connection = new SqlConnection(path);
                        connection.Open();
                        login = LoginBox.Text;
                        FIO = fioBox.Text;
                        age = ageBox.Text;
                        if (login.Length > 35 || FIO.Length>35)
                        {
                            MessageBox.Show("Слишком длинные значения!", "Неверный ввод");
                        }
                        else
                        {
                            DataTable dt = new DataTable();
                            string command = $"SELECT * FROM client WHERE login LIKE @login";
                            SqlCommand sqlCommand = new SqlCommand(command, connection);
                            sqlCommand.Parameters.AddWithValue("login", login);
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                            dt = new DataTable("client");
                            dataAdapter.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("Пользователь уже есть в базе данных!", "Не удалось зарегестрироваться!");
                            }
                            else
                            {
                                command = $"INSERT INTO client (FIO, age, login, password) VALUES (@FIO, @age, @login, @password)";

                                sqlCommand = new SqlCommand(command, connection);

                               
                                sqlCommand.Parameters.AddWithValue("FIO", FIO);
                                sqlCommand.Parameters.AddWithValue("age", age);
                                sqlCommand.Parameters.AddWithValue("login", login);
                                sqlCommand.Parameters.AddWithValue("password", password);
                                sqlCommand.ExecuteNonQuery();
                                connection.Close();
                                MainWindow mw = new MainWindow();
                                //mw.login = login;
                                Data.Login = login;
                                
                                mw.Show();
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не сходятся значения!", "Ошибка");
                    }
                }
            }
        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            Login ua = new Login();
            ua.Show();
            this.Close();
        }

        private void ageBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }

        private void fioBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}