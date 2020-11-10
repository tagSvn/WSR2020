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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
            //MainWindow.Register();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login, pass, name;
            login = TextLogin.Text;
            name = TextName.Text;
            pass = TextPass.Password;

            TextError.Text = MainWindow.Register(login,pass,name);
        }
    }
}
