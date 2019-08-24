using ConsoleApp1.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class CatlogContext
    {
        private readonly IMongoDatabase database;

        public CatlogContext()
        {
            var connectionString = @"mongodb://sameer-mongoapi:YZj9VHjnuVO1kefigjcwzdstxP42JVfuJqObUQkGxk2Y5TCocJHHL5C8oSUYYsMl1nbOMBRqwUr2wb7wzSIGPA==@sameer-mongoapi.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            var databaseName = "ecatlog";
            MongoClientSettings clientSettings = MongoClientSettings.FromConnectionString(connectionString);
            MongoClient client = new MongoClient(clientSettings);
            if (client != null)
            {

                this.database = client.GetDatabase(databaseName);
            }
        }

        public IMongoCollection<CatlogItem> CatlogItems
        {
            get
            {
                return this.database.GetCollection<CatlogItem>("CatlogItems");
            }
        }
    }
}
