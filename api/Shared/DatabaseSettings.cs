namespace api.Shared;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string EventCollectionName { get; set; } = null!;
    public string UserCollectionName { get; set; } = null!;
}