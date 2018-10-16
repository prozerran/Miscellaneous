
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    class Customer : TableEntity
    {
        public string name { get; set; }
        public string email { get; set; }

        public Customer() { }

        public Customer(string name, string email)
        {
            this.name = name;
            this.email = email;
            this.PartitionKey = "CUST";
            this.RowKey = email;
        }
    }

    class Tables
    {
        /*
        static void Main(string[] args)
        {
            var storage = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection"));

            var tableClient = storage.CreateCloudTableClient();
            var table = tableClient.GetTableReference("customers");

            table.CreateIfNotExists();

            // batch insert
            var batch = new TableBatchOperation();

            var cust1 = new Customer("name7", "name7@localhost.local");
            var cust2 = new Customer("name8", "name8@localhost.local");
            var cust3 = new Customer("name9", "name9@localhost.local");

            batch.InsertOrReplace(cust1);
            batch.InsertOrReplace(cust2);
            batch.InsertOrReplace(cust3);

            table.ExecuteBatch(batch);

            // display all rows in table
            var query = new TableQuery<Customer>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CUST"));

            foreach (var cust in table.ExecuteQuery(query))
            {
                Console.WriteLine(cust.email);
            }
            Console.ReadKey();
        }
        */

        static void InsertCustomer(CloudTable table, Customer cust)
        {
            table.Execute(TableOperation.InsertOrReplace(cust));
        }

        static string GetCustomer(CloudTable table, string partkey, string rowkey)
        {
            var result = table.Execute(TableOperation.Retrieve<Customer>(partkey, rowkey));
            return ((Customer)result.Result).name;
        }

        static void UpdateCustomer(CloudTable table, Customer cust)
        {
            table.Execute(TableOperation.Replace(cust));
        }

        static void DeleteCustomer(CloudTable table, Customer cust)
        {
            table.Execute(TableOperation.Delete(cust));
        }
    }
}
