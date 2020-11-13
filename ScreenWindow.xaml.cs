using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
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

namespace WpfApp1
{
    public class Cloth
    {
        public string Article { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Info { get; set; }
        public string Paint { get; set; }
        public Image image { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Price { get; set; }

    }
    public class Furn
    {
        public string Article { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Weight { get; set; }
        public Image image { get; set; }
        public float Price { get; set; }
    }
    public class Prod
    {
        public string Article { get; set; }
        public string Name { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Price { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для ScreenWindow.xaml
    /// </summary>
    /// 
    public class User
    {
        public static string login, name, role;
        public static Frame mainframe = new Frame();
        public static TextBlock textboxuser = new TextBlock();
        public static TextBlock textboxrole = new TextBlock();
        public static void SelectUser(int id) 
        {
            MySqlCommand sql = new MySqlCommand($"SELECT * FROM {MainWindow.usersdb} WHERE id = {id};", MainWindow.connection);
            MySqlDataReader reader = sql.ExecuteReader();
            reader.Read();
            login = Convert.ToString(reader.GetValue(1)); name = Convert.ToString(reader.GetValue(4)); role = Convert.ToString(reader.GetValue(3));
            reader.Close();
            switch (role)
            {
                case "Заказчик": mainframe.Navigate(new Pages.Screens.Screen_Client()); break;
                //case "Менеджер": mainframe.Navigate(new Pages.Screens.Screen_Manager()); break;
                //case "Кладовщик": mainframe.Navigate(new Pages.Screens.Screen_Storekeeper()); break;                                   //Функция определения пользователя
                //case "Дирекция": mainframe.Navigate(new Pages.Screens.Screen_Director()); break;
            }
            textboxuser.Text = "Пользователь: " + User.login;
            textboxrole.Text = role;
            
        }
    }
    public partial class ScreenWindow : Window
    {
        public ScreenWindow()
        {
            InitializeComponent();
            User.mainframe = FormRole;
            User.textboxuser = username;
            User.textboxrole = userrole;
            if (this.Visibility == Visibility.Visible) MainWindow.connection.Open();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.connection.Close();
            this.Close();
            Window screen = new Window();                                           
            screen = new MainWindow();
            screen.Visibility = Visibility.Visible;
        }//Функция выхода из экрана пользователя
    }

}
