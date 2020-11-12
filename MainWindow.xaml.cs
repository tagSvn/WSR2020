using System;
using System.Data;
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
using MySql.Data.MySqlClient;
using WpfApp1.Pages;
using System.Security.RightsManagement;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    /*class Temps {
        public static Frame frame1;
        public static Window Screenwindow = new Window();                                                 //Класс для преобразования нестатичных объектов
        public static Window thiswindow = new Window();
    } */
    public partial class MainWindow : Window
    {
        public static string dbname = "companydb";                                        //Переменные для конекта с базой sql dbname - название базы данных,
        public static string usersdb = "пользователи";                                                                         //usersdb - таблица с пользователями

        internal static string connect = $"server=localhost;user=root;database={dbname};password=nerovi28;";
        internal static MySqlConnection connection = new MySqlConnection(connect);
        internal static MySqlCommand sql = new MySqlCommand("", connection);
        internal static MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql);

        public static Frame frame1;
        public static Window Screenwindow = new Window();                                                 //Преобразования нестатичных объектов
        public static Window thiswindow = new Window();
        
        public MainWindow()
        {
            bool upconnect = true;                                                           //Переменная проверки доступности базы данных
            InitializeComponent();

            try { connection.Open(); }
            catch { MessageBox.Show("Нет соединения с базой данных"); upconnect = false;}
            finally { if (upconnect == true) { connection.Open(); frame1.Navigate(new AuthPage()); 
                    authbut.Visibility = Visibility.Visible; 
                    regbut.Visibility = Visibility.Visible; 
                        } 
                    }

            frame1 = frame;
            Screenwindow = new ScreenWindow();                                             //Присваивание статичным элементам значения
            thiswindow = this;
        //Mainwindow = WpfApp1.MainWindow();

       
            //ConsoleApp1.Program.MainMenu();
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            frame1.Navigate(new AuthPage());
        }

        void Button_Click_1(object sender, RoutedEventArgs e)
        {
            frame1.Navigate(new RegisterPage());
        }                    //Кнопки
        void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GoToScreen(1);
        }
        static void GoToScreen(int num)
        {
            User.SelectUser(num);
            Screenwindow.Show();                                            //Функция перехода на экран пользователя num - id 
            thiswindow.Hide();
            connection.Close();
        }
        public static string Register(string login = "", string pass = "", string name = "", string role = "Заказчик")
        {
            string error;
            MySqlCommand sql = new MySqlCommand($"SELECT Логин, Пароль FROM {usersdb}", connection);
            //MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql);

            DataTable dt = new DataTable();
            dataAdapter.SelectCommand = sql;        //Вставка команды
            dataAdapter.Fill(dt);
            var myData = dt.Select();

            if (login == "") { error = "Некорректный логин"; return error; }

            for (int i = 0; i < myData.Length; i++)
            {
                if (Convert.ToString(Convert.ToString(myData[i].ItemArray[0])).ToLower() == login.ToLower())            //Проверка логина на дубликат
                { 
                    error = "Этот логин уже занят";
                    //uqlogin = false;
                    return error;
                }
            }
            if (role != "Заказчик" && role != "Менеджер" && role != "Кладовщик" && role != "Дирекция") { error = "Неверная роль"; return error; }
            if (pass == "") { error = "Некорректный пароль"; return error; }                                                                        //Проверка остальных полей
            if (name == "") { error = "Некорректное имя"; return error; }

            
            sql = new MySqlCommand($"INSERT INTO {usersdb}(Логин,Пароль,Наименование,Роль) VALUES (\"{login}\",\"{pass}\",\"{name}\",\"{role}\");", connection);  //Добавление в базу
            sql.ExecuteNonQuery();
            error = "Пользователь создан";

            dt.Clear();
            frame1.Navigate(new AuthPage());
            dataAdapter.Dispose();
            return error;
        }
        public static string Login(string login ="", string pass="")
        {
            string error = "", role = "";
            int i, id = 0;

            //login = TextLogin.Text;
            //pass = TextPassword.Password;

            MySqlCommand sql = new MySqlCommand($"SELECT Логин, Пароль, Роль, Id FROM {usersdb}", MainWindow.connection);
            //MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();

            dataAdapter.SelectCommand = sql;
            dataAdapter.Fill(dt);

            var myData = dt.Select();      //dt = dataTable
            for (i = 0; i < myData.Length; i++)
            {
                if (Convert.ToString(myData[i].ItemArray[0]) == login) { error = "ok"; break; }                           //Проверка логина на существование
                else error = "Такого пользователя нет";
            }
            if (error == "ok")
                if (Convert.ToString(myData[i].ItemArray[1]) == pass)
                {                                                                                    //Проверка пароля и вход                                                                              //userScreen(Convert.ToString(myData[i].ItemArray[2]));
                    error = "Вход выполняется";
                    id = Convert.ToInt32(myData[i].ItemArray[3]);
                    role = Convert.ToString(myData[i].ItemArray[2]);
                }
                else error = "Пароль неверный";
            dt.Clear();
            dataAdapter.Dispose();
            if (id != 0 && error == "Вход выполняется") GoToScreen(id);
            return error;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
