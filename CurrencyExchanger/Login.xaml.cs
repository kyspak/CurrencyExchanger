using System.Data;
using System.Data.SqlClient;
using System.Windows;
namespace CurrencyExchanger
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        public string login;
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Data.adminIsChecked = admin.IsChecked;
            if (admin.IsChecked == false)
            {
                
                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                DataTable dt = new DataTable();
                connection.Open();
                login = LoginBox.Text;
                string password = ps.Password.ToString();

                string command = $"SELECT * FROM client WHERE login LIKE '{login}'";
                SqlCommand sqlCommand = new SqlCommand(command, connection);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dt = new DataTable("client");
                dataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                   
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    MainWindow mw = new MainWindow();
                    Data.Login = login;
                    mw.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Не удалось выполнить вход!");

                }
            }

            else
            {
                string path = @"Data Source=BASS\SQLEXPRESS; Initial catalog = CurrencyExchanger; Integrated Security=True";
                SqlConnection connection = new SqlConnection(path);
                DataTable dt = new DataTable();
                connection.Open();
                login = LoginBox.Text;
                string password = ps.Password.ToString();
                string command = $"SELECT * FROM admin WHERE (login LIKE '{login}') AND (password LIKE '{password}')";
                SqlCommand sqlCommand = new SqlCommand(command, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dt = new DataTable("admin");
                dataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    MainWindow mw = new MainWindow();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    Data.Login = login;
                    mw.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Не удалось выполнить вход!");
                }
            }

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Data.adminIsChecked = admin.IsChecked;
            Registration ur = new Registration();
            ur.Show();
            this.Close();
        }
    }
}
