using MySqlConnector;

namespace Server.Repositories
{
    public class Repository
    {
        protected MySqlConnection CreateConnection() => new("Server=localhost;User ID=root;Password=admin;Database=admin");
    }
}
