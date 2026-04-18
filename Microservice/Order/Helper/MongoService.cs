using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.Models;

namespace OrderService.Helper
{
    public class MongoContext
    {
        public IMongoCollection<Order> Orders { get; }

        public MongoContext(IOptions<MongoDbSetting> mongoDbSetting)
        {
            var mongoClient = new MongoClient(mongoDbSetting.Value.ConnectionURl);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSetting.Value.DatabaseName);
            var collectionName = string.IsNullOrWhiteSpace(mongoDbSetting.Value.CollectionName)
                ? "Orders"
                : mongoDbSetting.Value.CollectionName;

            Orders = mongoDatabase.GetCollection<Order>(collectionName);
        }
    }
}
