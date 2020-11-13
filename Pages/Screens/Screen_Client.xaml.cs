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
        public string selected = null;
        public List<Cloth> dataCloth = new List<Cloth>();
        public List<Furn> dataFurn = new List<Furn>();
        public List<Prod> dataProd = new List<Prod>();
        public Screen_Client()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)      //Кнопка "показать ткани"
        {
            selected = "TableClotch";
            OpenTable();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)     //Кнопка "показать фурнитуры"
        {
            selected = "TableFurn";
            OpenTable();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            selected = "TableProd";
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

            switch (selected)
            {
                case "TableClotch": selectedTable = "ткань"; break;       
                case "TableFurn": selectedTable = "фурнитура"; break;
                case "TableProd": selectedTable = "изделия"; break;
            }//Определение с какой бд брать данные

                MySqlCommand sql = new MySqlCommand($"SELECT * FROM {selectedTable};", MainWindow.connection);
                DataTable dt = new DataTable();
                MainWindow.dataAdapter.SelectCommand = sql;
                MainWindow.dataAdapter.Fill(dt);

            if (selected == "TableClotch")
            {
                if (dataCloth.Count == 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Cloth clotch1 = new Cloth();
                        
                        clotch1.Article = Convert.ToString(dt.Rows[i].ItemArray[0]); clotch1.Name = Convert.ToString(dt.Rows[i].ItemArray[1]);
                        clotch1.Color = Convert.ToString(dt.Rows[i].ItemArray[2]); clotch1.Paint = Convert.ToString(dt.Rows[i].ItemArray[3]);
                        clotch1.Info = Convert.ToString(dt.Rows[i].ItemArray[4]); clotch1.Length = Convert.ToSingle(dt.Rows[i].ItemArray[5]);    //Заполнение коллекции
                        clotch1.Width = Convert.ToSingle(dt.Rows[i].ItemArray[6]); clotch1.Price = Convert.ToSingle(dt.Rows[i].ItemArray[7]);
                        dataCloth.Add(clotch1);
                    }
                {
                    MainGrid.ItemsSource = dataCloth;
                    MainGrid.Columns[0].Header = "Артикл"; MainGrid.Columns[1].Header = "Наименование"; MainGrid.Columns[2].Header = "Цвет";
                    MainGrid.Columns[3].Header = "Рисунок"; MainGrid.Columns[4].Header = "Состав"; MainGrid.Columns[5].Header = "Длина";
                    MainGrid.Columns[6].Header = "Ширина"; MainGrid.Columns[7].Header = "Цена";
                }
            }//Заполнение таблицы тканей
            else if (selected == "TableFurn")
            {
                if (dataFurn.Count == 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Furn furn1 = new Furn();

                        furn1.Article = Convert.ToString(dt.Rows[i].ItemArray[0]); furn1.Name = Convert.ToString(dt.Rows[i].ItemArray[1]);
                        furn1.Length = Convert.ToSingle(dt.Rows[i].ItemArray[4]); furn1.Type = Convert.ToString(dt.Rows[i].ItemArray[2]);
                        furn1.Width = Convert.ToSingle(dt.Rows[i].ItemArray[3]); furn1.Price = Convert.ToSingle(dt.Rows[i].ItemArray[7]);
                        furn1.Weight = Convert.ToSingle(dt.Rows[i].ItemArray[5]);
                        dataFurn.Add(furn1);
                    }
                {
                    MainGrid.ItemsSource = dataFurn;
                    MainGrid.Columns[0].Header = "Артикл"; MainGrid.Columns[1].Header = "Наименование"; MainGrid.Columns[2].Header = "Тип";
                    MainGrid.Columns[3].Header = "Ширина"; MainGrid.Columns[4].Header = "Длина"; MainGrid.Columns[5].Header = "Вес";
                    MainGrid.Columns[6].Header = "Изображение"; MainGrid.Columns[7].Header = "Цена";
                }
            }//Заполнение таблицы фурнитуры
            else if (selected == "TableProd")
            {
                if (dataProd.Count == 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Prod prod1 = new Prod();
                        prod1.Article = Convert.ToString(dt.Rows[i].ItemArray[0]); prod1.Name = Convert.ToString(dt.Rows[i].ItemArray[1]);
                        prod1.Length = Convert.ToSingle(dt.Rows[i].ItemArray[3]);
                        prod1.Width = Convert.ToSingle(dt.Rows[i].ItemArray[2]);
                        //TableFurn.Items.Add(prod1);
                        dataProd.Add(prod1);
                    }
                //foreach (Prod prod in dataProd) selected.Items.Add(prod);
            }//Заполнение таблицы изделий

            MainGrid.Items.Refresh();
            MainWindow.dataAdapter.Dispose();
        }

        void SearchTable(string text)
        {
            List<Cloth> copylist;
            List<Furn> copylist1;                      //Листы под дубликаты
            List<Prod> copylist2;

            string ss = "";
            if (text == "") MainGrid.ItemsSource = null;
            else
            {
                if (selected == "TableClotch")
                {
                    copylist = dataCloth.ToList();                                //Дублирование коллекции и отображение дубликата
                    MainGrid.ItemsSource = copylist;
                    foreach (Cloth cloth in dataCloth.ToList())
                    {
                        ss = cloth.Name.ToLower();
                        if (ss.IndexOf(text.ToLower()) == -1) copylist.Remove(cloth);        //Пересчитывание в основной коллекции и удаление из дубликата
                    }
                }
                if (selected == "TableFurn")
                {
                    copylist1 = dataFurn.ToList();
                    MainGrid.ItemsSource = copylist1;
                    foreach (Furn furn in dataFurn.ToList())
                    {
                        ss = furn.Name.ToLower();
                        if (ss.IndexOf(text.ToLower()) == -1) copylist1.Remove(furn);
                    }
                }
                if (selected == "TableProd")
                {
                    copylist2 = dataProd.ToList();
                    MainGrid.ItemsSource = copylist2;
                    foreach (Prod prod in dataProd.ToList())
                    {
                        ss = prod.Name.ToLower();
                        if (ss.IndexOf(text.ToLower()) == -1) copylist2.Remove(prod);
                    }
                }
                MainGrid.Items.Refresh();
            }//Фильтрация по таблицам
        }//Функция поиска по открытой таблице
    }
}
