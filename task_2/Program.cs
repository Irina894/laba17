using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data;

namespace P03_SalesDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SalesContext())
            {
                Seeder.Seed(context);

                Console.WriteLine("--- Перевірка даних ---");

                var sales = context.Sales
                    .Include(s => s.Customer) 
                    .Include(s => s.Product)  
                    .Include(s => s.Store)   
                    .OrderByDescending(s => s.Date)
                    .Take(5)
                    .ToList();

                if (sales.Count == 0)
                {
                    Console.WriteLine("База даних порожня! Запустіть Seeder.");
                }
                else
                {
                    foreach (var sale in sales)
                    {
                        Console.WriteLine($"Sale #{sale.SaleId} | Date: {sale.Date.ToShortDateString()}");
                        Console.WriteLine($"   Product:  {sale.Product.Name} (${sale.Product.Price})");
                        Console.WriteLine($"   Customer: {sale.Customer.Name} ({sale.Customer.Email})");
                        Console.WriteLine($"   Store:    {sale.Store.Name}");
                        Console.WriteLine("--------------------------------------------------");
                    }
                }
            }
        }
    }
}