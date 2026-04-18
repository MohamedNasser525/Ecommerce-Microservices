namespace OrderService.Helper
{
    public class MongoDbSetting
    {
        public string ConnectionURl { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CollectionName { get; set; } = "Orders";
    }
}
