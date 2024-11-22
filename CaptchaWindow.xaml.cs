using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для CaptchaWindow.xaml
    /// </summary>
    public partial class CaptchaWindow : Window
    {
        private string _captchaText;
        public CaptchaWindow()
        {
            InitializeComponent();
            LoadCaptcha();
        }

        private void LoadCaptcha()
        {
            _captchaText = CaptchaGenerator.GenerateCaptchaText();
            var imageBytes = CaptchaGenerator.GetCaptchaImageAsByteArray(_captchaText);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                CaptchaImage.Source = image;
            }
        }

        private void ConfirmCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (CaptchaTextBox.Text == _captchaText)
            {
                MessageBox.Show("Капча подтверждена.");
                this.DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Неверная капча, попробуйте снова.");
                CaptchaTextBox.Clear();
                LoadCaptcha(); 
            }
        }
    }
}
