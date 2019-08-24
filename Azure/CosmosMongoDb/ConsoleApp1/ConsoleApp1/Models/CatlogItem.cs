using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Vendor
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
    class CatlogItem
    {
        public CatlogItem()
        {
            this.Vendors = new List<Vendor>();
        }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public List<Vendor> Vendors { get; set; }
    }
}
