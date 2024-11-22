using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _3._3pz
{
    public class DbInitializer
    {
        public static void Initialize(DbpContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(new[]
                {
                new Models.Role { RoleName = "admin" },
                new Models.Role { RoleName = "user" }
            });
                context.SaveChanges();
            }

            if (!context.Services.Any())
            {
                context.Services.AddRange(new[]
                {
                new Models.Service { ServiceName = "Водоснабжение", Description = "цена за км", Price = 1500000, ImagePath = ""},
                new Models.Service { ServiceName = "Электроснабжение", Description = "цена за км", Price = 2500000 },
                new Models.Service { ServiceName = "Проектирование и архитектурные услуги", Description = "цена за проект", Price = 1000000, ImagePath = "" },
                new Models.Service { ServiceName = "Монтаж окон и дверей", Description = "цена за единицу", Price = 20000, ImagePath = "" },
                new Models.Service { ServiceName = "Утепление фасадов и стен зданий", Description = "цена за кв/м", Price = 3200, ImagePath = "" }
            });
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                // генерация соли для каждого пользователя
                string salt1 = GenerateSalt();
                string salt2 = GenerateSalt();

                context.Users.AddRange(new[]
                    {
                new Models.User
                {
                    FirstName = "ad",
                    LastName = "ad",
                    Phone = "89221111111",
                    Salt = salt1,  
                    PasswordHash = ComputeSha256Hash("111", salt1), 
                    RoleID = 1
                },
                new Models.User
                {
                    FirstName = "test",
                    LastName = "test",
                    Phone = "89220000000",
                    Salt = salt2,  
                    PasswordHash = ComputeSha256Hash("111", salt2), 
                    RoleID = 2
                }
            });
                context.SaveChanges();
            }
            if (!context.Carts.Any())
            {
                context.Carts.Add(new Models.Cart { UserID = 1 });
                context.SaveChanges();
            }
        }

        public static string GenerateSalt(int size = 32)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[size];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        public static string ComputeSha256Hash(string rawData, string salt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string saltedData = rawData + salt;

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
