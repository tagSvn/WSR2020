using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    /*internal class User
    {
        public static User[] users = new User[2];
        public string Login;
        public string Pass;
        public string Name;
        public int id;

        public User(int id1, string l, string p) { id = id1; Login = l; Pass = p; }
        public User(int id1, string l, string p, string n) { id = id1; Login = l; Pass = p; Name = n; }
    } */
    class Program
    {
        /*static string connect = "server=localhost;user=root;database=testdb;password=nerovi28;";
        static MySqlConnection connection = new MySqlConnection(connect);

        public static void Menu(string st)
        {

            switch (st)
            {
                case "1": /*ret = "1"; login(); break;
                case "2": /*ret = "2";  register(); break;                                           //Функция меню
                case "3": /*ret = "3";  Environment.Exit(-666); break;
                case "4": /*ret = "4";  ShowDb("usersdb"); break;
            }
            //Console.WriteLine(ret);
            //return ret; 
        } 
        
        static void userScreen(string Role)
        {
            bool exit = false;
            if (Role == "Заказчик") 
                while (!exit)
                {
                    Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXX\nПока что тут ничего нет \n Выход - 0");
                    switch (Console.ReadLine())
                    {
                        
                        case "0": exit = true; break;
                    };
                }
            else if (Role == "Менеджер")
                while (!exit)
                {
                    Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXX\nВыгрузить информацию по тканям - 1 \nВыгрузить информацию по изделиям\n Выход - 0");
                    switch (Console.ReadLine())
                    {
                        case "1": ShowDb("clotch"); break;
                        case "2": ShowDb("prod"); break;
                        case "0": exit = true; break;                                                                     //Экран пользователей, поочерёдно
                    };
                }
            else if (Role == "Кладовщик")
                while (!exit)
                {
                    Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXX\nВыгрузить информацию по тканям - 1 \nВыгрузить информацию по фурнитуре - 2\n Выход - 0");
                    switch (Console.ReadLine())
                    {
                        case "1": ShowDb("clotch"); break;
                        case "2": ShowDb("furn"); break;
                        case "0": exit = true; break;
                    };
                }
            else if (Role == "Дирекция")
                while (!exit)
                {
                    Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXX\nВыгрузить информацию по тканям - 1 \nВыгрузить информацию по фурнитура - 2\nВыгрузить информацию по изделиям - 3 \nПоказать базу пользователей - 4\nДобавить нового пользователя - 5\nРедактировать профиль \nВыход - 0");
                    switch (Console.ReadLine())
                    {
                        case "1": ShowDb("clotch"); break;
                        case "2": ShowDb("furn"); break;
                        case "3": ShowDb("prod"); break;
                        case "4": ShowDb("usersdb"); break;
                        case "5": addUser(true); break;
                        case "6": addUser(false,true); break;
                        case "0": exit = true; break;
                    };
                }
            else Console.WriteLine("Ошибка: роль неизвестна или отсутствует."); 

            void addUser(bool add = false, bool update = false)
            {
                string data;
                object login, pass, name, role = "";
                string[] datas;
                bool exc = false ;
                int id= 0;
                if (add)
                {
                    Console.WriteLine("Введите логин, пароль, имя, роль через запятую");                             //Тут при добавлении
                    
                }
                else if (update)
                {
                    
                    while (true)
                    {
                        Console.WriteLine("Введите id пользователя");                                                  //Функция работы с пользователями директора
                        data = Console.ReadLine();
                        if (Int32.TryParse(data, out id)) break;
                        else Console.WriteLine("Неверный id");
                    }
                    MySqlCommand sql = new MySqlCommand($"SELECT * FROM usersdb WHERE id={id}; ", connection);
                    MySqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        login = reader.GetValue(1); pass = reader.GetValue(2); name = reader.GetValue(3); role = reader.GetValue(4);         //А тут при обновлении
                        if (Convert.ToString(login) == "") Console.WriteLine("Такого пользователя нет в базе\n");
                        else Console.WriteLine($"{login} | {pass} | {name} | {role}");
                    }
                    reader.Close();
                    Console.WriteLine("Введите новые данные через запятую");
                }
                data = Console.ReadLine();
                datas = data.Split(new char[] { ',' });
                try { login = datas[0]; pass = datas[1]; name = datas[2]; role = datas[3]; }
                catch { Console.WriteLine("Введённые данные некоректны\n"); exc = true; }                                                      //Ввод данных и проверки

                if (exc == false && datas[3] != "Заказчик" && datas[3] != "Менеджер" && datas[3] != "Кладовщик" && datas[3] != "Дирекция") { Console.WriteLine("Ошибка при вводе роли\n"); exc = true; }
                if (exc == false)
                {
                    login = datas[0]; pass = datas[1]; name = datas[2]; role = datas[3];
                    if (add) register(Convert.ToString(login), Convert.ToString(pass), Convert.ToString(name), Convert.ToString(role));
                    MySqlCommand sql = new MySqlCommand($"UPDATE testdb.usersdb SET Login = \"{login}\", " +
                        $"                                                                Password = \"{pass}\", " +
                        $"                                                                Name = \"{name}\", " +
                        $"                                                                Role = \"{role}\" " +
                        $"                                                                WHERE (id = {id})", connection);
                    if (update) 
                    {
                        try { int i = sql.ExecuteNonQuery(); }                                                                           //Непосредственно ввод в таблицу
                        catch(MySql.Data.MySqlClient.MySqlException ex) 
                        {
                            string exs = Convert.ToString(ex);
                            if (exs.IndexOf("Duplicate entry") != -1) Console.WriteLine("Логин дублируется\n");
                            else if (exs.IndexOf("Data too long for column") != -1) Console.WriteLine("Введено слишком большое значение\n");
                            else 
                            { 
                                sql.ExecuteNonQuery(); 
                                Console.WriteLine("Пользователь обновлён"); 
                            }
                        }
                    }
                }
                
            }
        }

        static bool login()
        {
            string login;
            string pass;
            string truepas = "";
            bool islogin = false;
            int i;

            Console.WriteLine("\nВведите логин");
            login = Console.ReadLine();
            Console.WriteLine("Введите пароль");
            pass = Console.ReadLine();

            MySqlCommand sql = new MySqlCommand("SELECT Login, Password, Role FROM usersdb", connection);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            var myData = dt.Select();      //dt = dataTable
            for (i = 0; i < myData.Length; i++)
            {
                    if (Convert.ToString(myData[i].ItemArray[0]) == login)                           //Проверка логина на существование
                    {
                        islogin = true;
                        truepas = "";
                        break;
                    } else truepas = "Такого пользователя нет";
            }

            if (islogin)
            {
                for (int j = 0; j < myData[1].ItemArray.Length; j++)
                {
                    if (Convert.ToString(myData[i].ItemArray[1]) == pass)
                    {
                        Console.WriteLine($"Вы авторизированы, {login}");                         //Проверка пароля и вход
                        userScreen(Convert.ToString(myData[i].ItemArray[2]));
                        truepas = "";
                        break;
                    } else truepas = "Пароль неверный";
                }
            }
            Console.WriteLine(truepas);
            
            dt.Clear();
            return true;
        }
        static string register(string login="", string pass="", string name="",string role="Заказчик")
        {
            bool uqlogin = false;
            MySqlCommand sql = new MySqlCommand("SELECT Login, Password FROM usersdb", connection);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            var myData = dt.Select();

            if (login =="") Console.WriteLine("Необходимо ввести желаемый логин, пароль и ваше имя\nВведите логин");
            while (!uqlogin)
            {
                while (login == "") { login = Console.ReadLine(); if (login == "") Console.WriteLine("Поле не должно быть пустым"); }
                for (int i = 0; i < myData.Length; i++)
                {
                    if (Convert.ToString(Convert.ToString(myData[i].ItemArray[0])).ToLower() == login.ToLower())                                                                //Проверка логина на дубликат
                    {
                        Console.WriteLine("Этот логин уже занят");
                        uqlogin = false;
                        login = Console.ReadLine();
                        break;
                    }
                    else uqlogin = true;
                }
            } 

            if (pass=="") Console.WriteLine("Введите пароль");
            while (pass == "") { pass = Console.ReadLine(); if (pass == "") Console.WriteLine("Поле не должно быть пустым"); }
            if (name=="") Console.WriteLine("Введите имя");
            while (name == "") { name = Console.ReadLine(); if (name == "") Console.WriteLine("Поле не должно быть пустым"); }

            sql = new MySqlCommand($"INSERT INTO usersdb(Login,Password,Name,Role) VALUES (\"{login}\",\"{pass}\",\"{name}\",\"{role}\");", connection);
            sql.ExecuteNonQuery();  
            Console.WriteLine("Пользователь создан"); 
            
            dt.Clear();
            return "ss";
        } 
        public static void ShowDb(String database)
        {
            
            MySqlCommand sql = new MySqlCommand($"SELECT * FROM {database}", connection);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            var myData = dt.Select();      //dt = dataTable
            for (int i = 0; i < myData.Length; i++)
            {     
                for (int j = 0; j < myData[i].ItemArray.Length; j++)                                             //Выгрузка базы
                {
                    if (Convert.ToString(myData[i].ItemArray[j]) == "") Console.Write("Null|");
                    else  Console.Write(myData[i].ItemArray[j] + "|");
                }
                Console.WriteLine("\n");
            }
        } */

        internal static void MainMenu()
        {
            //connection.Open();

            //while (true) 
            //{
                //Console.WriteLine("Консольная версия задания 1.2 \nНажмите 1 для авторизации\nНажмите 2 для регистрации \nНажмите 3 для выхода");
                //Menu(Console.ReadLine()); 
            //} 
        }
    }
}
