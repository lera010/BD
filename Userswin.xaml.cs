using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace _3._3pz
{
    /// <summary>
    /// Логика взаимодействия для Userswin.xaml
    /// </summary>
    public partial class Userswin : Window
    {
        private DbpContext _context;
        public Userswin()
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
            ServiceDG.ItemsSource = services;
        }

        private void ButtonAddCart_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
