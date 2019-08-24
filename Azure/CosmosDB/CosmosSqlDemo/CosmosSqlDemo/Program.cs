using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;

namespace CosmosSqlDemo
{
    class Program
    {
        static DocumentClient client;
        static string endPoint;
        static string authkey;
        static void Main(string[] args)
        {
            endPoint = @"https://sameer-coresql.documents.azure.com:443/";
            authkey = @"0diRSi7XWK9wcBlBv9sMKEWDTQWs1C00VKxzZzXh1Uv7Z9DloDNupHGwg76LcnFbxUEQ5oHoc7ACvdIrWG2rKQ==";

            client = new DocumentClient(new Uri(endPoint), authkey);

            //create database
            //   CreateDatabaseAsync("eshopdb").Wait();
            //create  collection
            // CreateCollectionAsync("eshopdb", "products", "/Category").Wait();

            //  var product = new { Name = "Thumups", Category = "Drinks", Price = 40, Quantity = 50 };
            //  CreateDocumentAsync("eshopdb", "products", product).Wait();

            ReadDocumentAsync("eshopdb", "products").Wait();
            Console.ReadLine();
        }

        static async Task CreateDatabaseAsync(string dbName)
        {
            await client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database { Id = dbName });

        }

        static async Task CreateCollectionAsync(string dbName, string collectionName, string partitionKey)
        {
            DocumentCollection documentCollection = new DocumentCollection();
            documentCollection.Id = collectionName;
            documentCollection.PartitionKey.Paths.Add(partitionKey);

            await client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(dbName)
               , documentCollection
               , new RequestOptions { OfferThroughput = 500 });

        }

        static async Task CreateDocumentAsync(string dbName, string collectionName, dynamic item)
        {
            await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(dbName, collectionName), item,
                 new RequestOptions { PartitionKey = new PartitionKey(item.Category), ConsistencyLevel = ConsistencyLevel.ConsistentPrefix });
        }

        static async Task ReadDocumentAsync(string dbName, string collectionName)
        {
            string continuationToken = null;
            do
            {
                var feed = await client.ReadDocumentFeedAsync(
                              UriFactory.CreateDocumentCollectionUri(dbName, collectionName)
                             , new FeedOptions { MaxItemCount = 10, RequestContinuation = continuationToken });

                continuationToken = feed.ResponseContinuation;
                foreach (Document item in feed)
                {
                    Console.WriteLine(item);
                }
            }
            while (continuationToken != null);

        }
    }
}
