namespace web_app_dev_back.Models
{
    public class Database
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UsersCollectionName { get; set; } = null!;

        public string OrdersCollectionName { get; set; } = null!;

        public string JobsCollectionName { get; set; } = null!;

        public string NotificationsCollectionName { get; set; } = null!;
    }
}
