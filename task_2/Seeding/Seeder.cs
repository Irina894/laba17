using System;
using System.Linq;
using System.Collections.Generic;
using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase
{
    public static class Seeder
    {
        public static void Seed(SalesContext context)
        {
            if (context.Products.Any()) return;

            var random = new Random();

            var products = new List<Product>();
            string[] productNames = { "CPU", "Motherboard", "GPU", "RAM", "SSD", "HDD", "Monitor", "Keyboard", "Mouse", "Headset" };

            for (int i = 0; i < 20; i++)
            {
                products.Add(new Product
                {
                    Name = productNames[random.Next(productNames.Length)] + " " + random.Next(100, 900),
                    Quantity = random.NextDouble() * 100,
                    Price = (decimal)(random.NextDouble() * 1000 + 10)
                });
            }
            context.Products.AddRange(products);

            var stores = new List<Store>();
            string[] storeNames = { "Rozetka", "Comfy", "Foxtrot", "Citrus", "Moyo", "Brain" };

            foreach (var name in storeNames)
            {
                stores.Add(new Store { Name = name });
            }
            context.Stores.AddRange(stores);

            var customers = new List<Customer>();
            string[] firstNames = { "Ivan", "Petro", "Oksana", "Maria", "Andriy", "Svitlana" };
            string[] lastNames = { "Kovalenko", "Bondar", "Melnyk", "Shevchenko", "Boyko" };

            for (int i = 0; i < 30; i++)
            {
                string fName = firstNames[random.Next(firstNames.Length)];
                string lName = lastNames[random.Next(lastNames.Length)];

                customers.Add(new Customer
                {
                    Name = $"{fName} {lName}",
                    Email = $"{fName.ToLower()}.{lName.ToLower()}_{random.Next(1000)}@gmail.com",
                    CreditCardNumber = $"{random.Next(1000, 9999)}-{random.Next(1000, 9999)}-{random.Next(1000, 9999)}-{random.Next(1000, 9999)}"
                });
            }
            context.Customers.AddRange(customers);

            context.SaveChanges();

            var sales = new List<Sale>();

            for (int i = 0; i < 100; i++)
            {
                sales.Add(new Sale
                {
                    Date = DateTime.Now.AddDays(-random.Next(1, 365)),
                    Product = products[random.Next(products.Count)],
                    Customer = customers[random.Next(customers.Count)],
                    Store = stores[random.Next(stores.Count)]
                });
            }
            context.Sales.AddRange(sales);

            context.SaveChanges();
            Console.WriteLine("Database seeded successfully!");
        }
    }
}