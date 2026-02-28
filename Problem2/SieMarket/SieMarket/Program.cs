using System;
using System.Collections.Generic;
using System.Linq;

namespace SieMarketApp
{
    public class Customer
    {
        public string Name { get; set; }
    }

    public class OrderItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;
    }

    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public Customer Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public decimal CalculateFinalPrice()
        {
            decimal total = Items.Sum(item => item.Subtotal);
            if (total > 500)
            {
                return total * 0.9m; 
            }
            return total;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var orders = GetSampleData();

            string topSpender = GetTopSpenderName(orders);
            Console.WriteLine($"Top Spender: {topSpender}");
            Console.WriteLine("---------------------------");

            DisplayPopularProducts(orders);

            Console.WriteLine("\npress any key to close");
            Console.ReadKey();
        }

        public static string GetTopSpenderName(List<Order> orders)
        {
            return orders
                .GroupBy(o => o.Customer.Name)
                .Select(g => new {
                    Name = g.Key,
                    Total = g.Sum(order => order.CalculateFinalPrice())
                })
                .OrderByDescending(x => x.Total)
                .FirstOrDefault()?.Name ?? "no client";
        }
        public static void DisplayPopularProducts(List<Order> orders)
        {
            var popular = orders
                .SelectMany(o => o.Items)
                .GroupBy(i => i.ProductName)
                .Select(g => new {
                    Product = g.Key,
                    Qty = g.Sum(i => i.Quantity)
                })
                .OrderByDescending(x => x.Qty);

            Console.WriteLine("Popular products leaderboard:");
            foreach (var p in popular)
            {
                Console.WriteLine($"- {p.Product}: {p.Qty} units");
            }
        }

        static List<Order> GetSampleData()
        {
            var c1 = new Customer { Name = "Andrei" };
            var c2 = new Customer { Name = "Maria" };

            return new List<Order>
            {
                new Order {
                    Customer = c1,
                    Items = new List<OrderItem> {
                        new OrderItem { ProductName = "Laptop", Quantity = 1, UnitPrice = 600 },
                        new OrderItem { ProductName = "Mouse", Quantity = 2, UnitPrice = 25 }
                    }
                },
                new Order {
                    Customer = c2,
                    Items = new List<OrderItem> {
                        new OrderItem { ProductName = "Laptop", Quantity = 1, UnitPrice = 600 },
                        new OrderItem { ProductName = "Monitor", Quantity = 1, UnitPrice = 200 }
                    }
                }
            };
        }
    }
}
