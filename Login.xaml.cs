using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private DbpContext _context;
        private int _loginAttempts = 0; // cчетчик попыток

        public Login()
        {
            InitializeComponent();
            _context = new DbpContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите имя пользователя и пароль.");
                return;
            }

            var user = _context.Users.FirstOrDefault(u => u.FirstName == username);

            if (user != null)
            {
                string salt = user.Salt;
                string hashedPassword = DbInitializer.ComputeSha256Hash(password, salt);

                if (user.PasswordHash == hashedPassword)
                {
                    if (user.RoleID == 1)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        Userswin userwin = new Userswin();
                        userwin.Show();
                        this.Close();
                    }
                }
                else
                {
                    _loginAttempts++;
                    MessageBox.Show("Неверный пароль.");

                    if (_loginAttempts >= 3)
                    {
                        if (ShowCaptchaWindow()) 
                        {
                            _loginAttempts = 0; 
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден.");
            }
        }

        private bool ShowCaptchaWindow()
        {
            CaptchaWindow captchaWindow = new CaptchaWindow();
            return captchaWindow.ShowDialog() == true;
        }
    }

}
