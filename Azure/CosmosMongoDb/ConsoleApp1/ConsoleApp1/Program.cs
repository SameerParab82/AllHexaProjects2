using ConsoleApp1.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static CatlogContext catlogContext;
        static void Main(string[] args)
        {
            catlogContext = new CatlogContext();

            CatlogItem catlogItem = new CatlogItem
            {
                Name = "Orange",
                Price = 80,
                Quantity = 50,
                Vendors = new List<Vendor>
            {
                new Vendor{Id= 1 , Name="ABC Expoert"},
                new Vendor{Id= 2 , Name="PQR Expoert"},
                new Vendor{Id= 3 , Name="XYZ Expoert"},
                new Vendor{Id= 4 , Name="MNO Expoert"}
            }

            };
            InsertItem(catlogItem);
            GetCatlogItem();

            //same you can try for update delete

            Console.ReadLine();
        }

        private static void InsertItem(CatlogItem catlogItem)
        {
            catlogContext.CatlogItems.InsertOne(catlogItem);
        }

        private static void GetCatlogItem()
        {
            var items = catlogContext.CatlogItems.FindAsync<CatlogItem>(FilterDefinition<CatlogItem>.Empty);
            var result = items.Result.ToList();
            foreach (CatlogItem item in result)
            {
                Console.WriteLine("Id ={0} Price={1},Qunatity={2}",item.Id, item.Price, item.Quantity);
                
            }

        }

    }
}
