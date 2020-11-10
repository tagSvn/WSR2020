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
using System.Windows.Resources;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Data;

namespace WpfApp1.Pages.Screens
{
    /// <summary>
    /// Логика взаимодействия для Screen_Client.xaml
    /// </summary>
    /// 
    
    public partial class Screen_Client : Page
    {
        public ListView selected = null;
        public List<Cloth> dataCloth = new List<Cloth>();
        public List<Furn> dataFurn = new List<Furn>();
        public List<Prod> dataProd = new List<Prod>();
        public Screen_Client()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)      //Кнопка "показать ткани"
        {
            selected = TableClotch;
            OpenTable();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)     //Кнопка "показать фурнитуры"
        {
            selected = TableFurn;
            OpenTable();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            selected = TableProd;
            OpenTable();
        }//Кнопка "Показать изделия"

        private void Find_Button(object sender, RoutedEventArgs e)
        {
            if (selected != null)
            {
                string findText = FindBox.Text;
                SearchTable(findText);
            }
            //else MessageBox.Show("Необходимо выбрать таблицу");
        }    //Кнопка поиска

        void OpenTable()                                  //Открытие таблицы
        {
            string selectedTable = "";
            TableClotch.Visibility = Visibility.Hidden;                  //Переделать на массив с перечислением и убиранием таблиц
            TableFurn.Visibility = Visibility.Hidden;
            TableProd.Visibility = Visibility.Hidden;
            selected.Visibility = Visibility.Visible;
            selected.Items.Clear(); 

            switch (selected.Name)
            {
                case "TableClotch": selectedTable = "clotch"; break;       
                case "TableFurn": selectedTable = "furn"; break;
                case "TableProd": selectedTable = "prod"; break;
            }//Определение с какой бд брать данные

                MySqlCommand sql = new MySqlCommand($"SELECT * FROM {selectedTable} LIMIT 10;", MainWindow.connection);
                DataTable dt = new DataTable();
                MainWindow.dataAdapter.SelectCommand = sql;
                MainWindow.dataAdapter.Fill(dt);

            if (selected.Name == "TableClotch")
            {
                if (dataCloth.Count == 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Cloth clotch1 = new Cloth();
                        
                        clotch1.Article = Convert.ToInt32(dt.Rows[i].ItemArray[0]); clotch1.Name = Convert.ToString(dt.Rows[i].ItemArray[1]);
                        clotch1.Color = Convert.ToString(dt.Rows[i].ItemArray[2]); clotch1.Paint = Convert.ToString(dt.Rows[i].ItemArray[3]);
                        clotch1.Info = Convert.ToString(dt.Rows[i].ItemArray[4]); clotch1.Length = Convert.ToInt32(dt.Rows[i].ItemArray[5]);
                        clotch1.Width = Convert.ToInt32(dt.Rows[i].ItemArray[6]); clotch1.Price = Convert.ToInt32(dt.Rows[i].ItemArray[7]);
                        clotch1.image = new Image(); clotch1.image.Source = new BitmapImage(new Uri(@"C:\1015.jpg", UriKind.RelativeOrAbsolute));
                        clotch1.image.Stretch = Stretch.Fill;
                        dataCloth.Add(clotch1);
                    }
                foreach (Cloth clot in dataCloth) selected.Items.Add(clot);
            }//Заполнение таблицы тканей
            else if (selected.Name == "TableFurn")
            {
                if (dataFurn.Count == 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Furn furn1 = new Furn();
                        furn1.Article = Convert.ToString(dt.Rows[i].ItemArray[0]); furn1.Name = Convert.ToString(dt.Rows[i].ItemArray[1]);
                        furn1.Length = Convert.ToInt32(dt.Rows[i].ItemArray[3]); furn1.Type = Convert.ToString(dt.Rows[i].ItemArray[4]);
                        furn1.Width = Convert.ToInt32(dt.Rows[i].ItemArray[2]); furn1.Price = Convert.ToInt32(dt.Rows[i].ItemArray[5]);
                        //TableFurn.Items.Add(furn1);
                        dataFurn.Add(furn1);
                    }
                foreach (Furn furn in dataFurn) selected.Items.Add(furn);
            }//Заполнение таблицы фурнитуры
            else if (selected.Name == "TableProd")
            {
                if (dataProd.Count == 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Prod prod1 = new Prod();
                        prod1.Article = Convert.ToString(dt.Rows[i].ItemArray[0]); prod1.Name = Convert.ToString(dt.Rows[i].ItemArray[1]);
                        prod1.Length = Convert.ToInt32(dt.Rows[i].ItemArray[2]);
                        prod1.Width = Convert.ToInt32(dt.Rows[i].ItemArray[3]); prod1.Price = Convert.ToInt32(dt.Rows[i].ItemArray[4]);
                        //TableFurn.Items.Add(prod1);
                        dataProd.Add(prod1);
                    }
                foreach (Prod prod in dataProd) selected.Items.Add(prod);
            }//Заполнение таблицы изделий
                
            selected.Items.Refresh();
            MainWindow.dataAdapter.Dispose();
        }

        void SearchTable(string text)
        {
            string ss = "";
            if (text == "") selected.Items.Clear();   
            else
            {
                if (selected.Name == "TableClotch")
                    foreach (Cloth cloth in dataCloth)
                    {
                        ss = cloth.Name.ToLower();
                        if (ss.IndexOf(text.ToLower()) == -1) selected.Items.Remove(cloth);
                    }
                        
                if (selected.Name == "TableFurn")
                    foreach (Furn furn in dataFurn)
                    {
                        ss = furn.Name.ToLower();
                        if (ss.IndexOf(text.ToLower()) == -1) selected.Items.Remove(furn);
                    }
                if (selected.Name == "TableProd")
                    foreach (Prod prod in dataProd)
                    {
                        ss = prod.Name.ToLower();
                        if (ss.IndexOf(text.ToLower()) == -1) selected.Items.Remove(prod); 
                    }
            }//Фильтрация по таблицам
        }//Функция поиска по открытой таблице
    }
}
