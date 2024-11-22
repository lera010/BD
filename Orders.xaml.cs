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
using System;
using System.Linq;


namespace _3._3pz
{
    public partial class Orders : Window
    {
        private readonly DbpContext _context;

        public Orders()
        {
            InitializeComponent();
            using (var context = new DbpContext())
            {
                DbInitializer.Initialize(context);
            }
            _context = new DbpContext();
            LoadOrders();
        }

        private void LoadOrders()
        {
            var query = from user in _context.Users
                        join role in _context.Roles on user.RoleID equals role.RoleID
                        select new UserModel
                        {
                            UserID = user.UserID,
                            FullName = user.FirstName + " " + user.LastName,
                            Phone = user.Phone,
                            RoleName = role.RoleName
                        };

            OrderDataGrid.ItemsSource = query.ToList();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string salt = DbInitializer.GenerateSalt();

                if (combo2.SelectedItem is ComboBoxItem selectedRole)
                {
                    int role = int.Parse(selectedRole.Tag.ToString());

                    var newUser = new Models.User
                    {
                        FirstName = NameBox.Text,
                        LastName = LastNameBox.Text,
                        Phone = PhoneBox.Text,
                        Salt = salt,
                        PasswordHash = DbInitializer.ComputeSha256Hash(PasswordBox.Text, salt),
                        RoleID = role
                    };

                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    LoadOrders();
                    MessageBox.Show("Пользователь добавлен успешно");
                }
                else
                {
                    MessageBox.Show("выберите роль.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления пользователя: {ex.Message}");
            }
        }

        private void UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                var selectedUserId = ((dynamic)OrderDataGrid.SelectedItem).UserID;

                var user = _context.Users.Find(selectedUserId);
                if (user != null)
                {
                    try
                    {
                        user.FirstName = NameBox.Text;
                        user.LastName = LastNameBox.Text;
                        user.Phone = PhoneBox.Text;

                        if (combo2.SelectedItem is ComboBoxItem selectedRole)
                        {
                            user.RoleID = int.Parse(selectedRole.Tag.ToString());
                        }

                        _context.SaveChanges();
                        LoadOrders();
                        MessageBox.Show("Пользователь обновлен успешно");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления пользователя: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("выберите пользователя для обновления.");
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                var selectedUserId = ((dynamic)OrderDataGrid.SelectedItem).UserID;

                var user = _context.Users.Find(selectedUserId);
                if (user != null)
                {
                    try
                    {
                        _context.Users.Remove(user);
                        _context.SaveChanges();
                        LoadOrders();
                        MessageBox.Show("Пользователь удален успешно!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления пользователя: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("выберите пользователя для удаления.");
            }
        }
    }
}
