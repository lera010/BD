using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class CaptchaGenerator
{
    private static readonly Random random = new Random();
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string GenerateCaptchaText()
    {
        int length = random.Next(4, 8);
        char[] text = new char[length];
        for (int i = 0; i < length; i++)
        {
            text[i] = chars[random.Next(chars.Length)];
        }
        return new string(text);
    }

    public static Bitmap GenerateCaptchaImage(string text)
    {
        int width = 200;
        int height = 150;
        Bitmap bitmap = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(System.Drawing.Color.White);

            //фоновый шум
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.LightGray))
            {
                for (int i = 0; i < 1000; i++)
                {
                    int x = random.Next(width);
                    int y = random.Next(height);
                    g.FillRectangle(brush, x, y, 1, 1);
                }
            }

            //текст капчи
            int xPos = 20;
            foreach (char c in text)
            {
                int fontSize = random.Next(15, 25);
                using (Font font = new Font("Arial", fontSize, FontStyle.Bold))
                {
                    g.TranslateTransform(xPos, 40);
                    g.RotateTransform(random.Next(-15, 15));
                    g.DrawString(c.ToString(), font, Brushes.Black, 0, 0);
                    g.ResetTransform();
                    xPos += fontSize - 3;
                }
            }

            //передний шум в виде линий
            using (Pen pen = new Pen(System.Drawing.Color.Gray, 1))
            {
                for (int i = 0; i < 5; i++)
                {
                    int x1 = random.Next(width);
                    int y1 = random.Next(height);
                    int x2 = random.Next(width);
                    int y2 = random.Next(height);
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }
        return bitmap;
    }

    public static byte[] GetCaptchaImageAsByteArray(string text)
    {
        using (Bitmap bitmap = GenerateCaptchaImage(text))
        using (MemoryStream ms = new MemoryStream())
        {
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
