namespace GameServer.WebApi.Models
{
    public class GameDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; } = string.Empty;
    }
}
