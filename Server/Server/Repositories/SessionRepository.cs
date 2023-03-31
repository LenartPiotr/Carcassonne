using Server.Models;

namespace Server.Repositories
{
    public class SessionRepository : Repository
    {
        MySqlConnector.MySqlConnection connection;
        public SessionRepository()
        {
            connection = CreateConnection();
        }

        public string CreateUserSession(User user, int durationMinutes)
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "select CreateUserSession(@user, @min)";
            command.Parameters.AddWithValue("@user", user.IdUser);
            command.Parameters.AddWithValue("@min", durationMinutes);

            var reader = command.ExecuteReader();

            reader.Read();
            string uuid = reader.GetString(0);

            connection.Clone();

            return uuid;
        }

        public bool GetUserSession(User user, out string uuid)
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "select GetUserSession(@user)";
            command.Parameters.AddWithValue("@user", user.IdUser);

            var reader = command.ExecuteReader();

            reader.Read();
            uuid = reader.GetString(0);

            connection.Clone();

            return uuid != "-1";
        }
    }
}
