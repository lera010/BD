using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static _3._3pz.Models;

namespace _3._3pz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbpContext _context;
        public MainWindow()
        {
            InitializeComponent();

            using (var context = new DbpContext())
            {
                DbInitializer.Initialize(context);
            }
            _context = new DbpContext();
            LoadServices();
        }

        private void LoadServices()
        {
            var services = _context.Services.ToList();
            ServiceDataGrid.ItemsSource = services;
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            try
            {
                var newService = new Models.Service
                {
                    ServiceName = ServiceName1.Text,
                    Description = Description1.Text,
                    Price = decimal.Parse(Price1.Text),
                    ImagePath = ""
                };

                _context.Services.Add(newService);
                _context.SaveChanges();

                LoadServices();  
                MessageBox.Show("Услуга добавлена успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления услуги: {ex.Message}");
            }
        }

        private void Button_delete(object sender, RoutedEventArgs e)
        {
            if (ServiceDataGrid.SelectedItem is Models.Service selectedService)
            {
                _context.Services.Remove(selectedService);
                _context.SaveChanges();

                LoadServices();  
                MessageBox.Show("Услуга удалена успешно!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу для удаления.");
            }
        }

        private void Button_update(object sender, RoutedEventArgs e)
        {
            if (ServiceDataGrid.SelectedItem is Models.Service selectedService)
            {
                try
                {
                    selectedService.ServiceName = ServiceName1.Text;
                    selectedService.Description = Description1.Text;
                    selectedService.Price = decimal.Parse(Price1.Text);

                    _context.SaveChanges();

                    LoadServices();  
                    MessageBox.Show("Услуга обновлена успешно!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обновления услуги: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу для обновления.");
            }
        }
        private void comboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSort = (ComboBoxItem)comboSort.SelectedItem;
            string sortBy = selectedSort.Tag.ToString();

            if (sortBy == "PriceAsc")
            {
                ServiceDataGrid.ItemsSource = _context.Services.OrderBy(s => s.Price).ToList();
            }
            else if (sortBy == "PriceDesc")
            {
                ServiceDataGrid.ItemsSource = _context.Services.OrderByDescending(s => s.Price).ToList();
            }
            else if (sortBy == "ID")
            {
                ServiceDataGrid.ItemsSource = _context.Services.OrderBy(s => s.ServiceID).ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)//кнопка сбросить
        {
           LoadServices();
        }

        private void Button_export(object sender, RoutedEventArgs e)
        {
            this.ServiceDataGrid.SelectAllCells();
            this.ServiceDataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, this.ServiceDataGrid);
            this.ServiceDataGrid.SelectAllCells();
            String res = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            try
            {
                using (StreamWriter sw = new StreamWriter("wpfdata.csv", false, new System.Text.UTF8Encoding(true)))//запись в кодировке UTF8, потому что иначе выводятся непонятные символы
                {
                    sw.WriteLine(res);
                }
                Process.Start("wpfdata.csv"); //файл wpfdata.csv это файл экпорта, wpfdata1.csv файл импорта
            }
            catch (Exception ex) { }
        }
        private void Button_import(object sender, RoutedEventArgs e)//кнопка импорта
        {
            try
            {
                if (!File.Exists("wpfdata1.csv"))
                {
                    MessageBox.Show("файла CSV нет");
                    return;
                }

                var importedServices = new List<Models.Service>();
                var lines = File.ReadAllLines("wpfdata1.csv"); //файл wpfdata.csv это файл экпорта, wpfdata1.csv файл импорта

                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var values = line.Split(',');

                    if (values.Length < 4)
                    {
                        continue;
                    }

                    decimal price;
                    if (!decimal.TryParse(values[3], out price))
                    {
                        continue;
                    }

                    var service = new Models.Service
                    {
                        ServiceName = values[1],
                        Description = values[2],
                        Price = price
                    };

                    importedServices.Add(service);
                }

                ServiceDataGrid.ItemsSource = importedServices;

                MessageBox.Show("Данные успешно загружены из CSV и отображены в таблице!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта данных: {ex.Message}");
            }
        }

        private void Button_Orders(object sender, RoutedEventArgs e)
        {
            Orders ordersWindow = new Orders();
            ordersWindow.Show();
        }


        private void Button_LoadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                Service service = ServiceDataGrid.SelectedItem as Service;

                if (service != null)
                {
                    service.ImagePath = imagePath; 
                    _context.SaveChanges(); 
                    LoadServices();
                }
            }
        }

    }

}