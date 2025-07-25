using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CurrencyExchanger
{
    public static class Data
    {
        public static string Login { get; set; }
        public static float Balance { get; set; }
        public static float FromRate { get; set; }
        public static float ToRate { get; set; }
        public static string FIO { get; set; }
        public static DateTime Date { get; set; }
        public static int client_id { get; set; }
        public static int currency_id { get; set; }
        public static int operation_id { get; set; }
        public static float count { get; set; }
        public static bool? adminIsChecked { get; set; }
        public static string post { get; set; }
       
    }
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        
    }
}
