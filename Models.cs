using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._3pz
{
    public class Models
    {
        public class Role
        {
            public int RoleID { get; set; }
            public string RoleName { get; set; }

            public ICollection<User> Users { get; set; }
        }

        public class User
        {
            public int UserID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public string PasswordHash { get; set; }
            public string Salt { get; set; }  
            public int RoleID { get; set; }
            public Role Role { get; set; }

            public ICollection<Cart> Carts { get; set; }
            public ICollection<Order> Orders { get; set; }
        }

        public class Service
        {
            public int ServiceID { get; set; }
            public string ServiceName { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string ImagePath { get; set; }

            public ICollection<OrderItem> OrderItems { get; set; }
        }


        public class Cart
        {
            public int CartID { get; set; }
            public int UserID { get; set; }
            public User User { get; set; }

            public ICollection<CartItem> CartItems { get; set; }
        }

        public class CartItem
        {
            public int CartItemID { get; set; }
            public int CartID { get; set; }
            public Cart Cart { get; set; }
            public int ServiceID { get; set; }
            public Service Service { get; set; }
            public int Quantity { get; set; }
        }

        public class Order
        {
            public int OrderID { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalAmount { get; set; }
            public int UserID { get; set; }
            public User User { get; set; }

            public ICollection<OrderItem> OrderItems { get; set; }
        }

        public class OrderItem
        {
            public int OrderItemID { get; set; }
            public int OrderID { get; set; }
            public Order Order { get; set; }
            public int ServiceID { get; set; }
            public Service Service { get; set; }
            public int Quantity { get; set; }
        }
    }
}
