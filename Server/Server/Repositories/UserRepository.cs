using Server.Exceptions;
using Server.Models;

namespace Server.Repositories
{
    public class UserRepository : Repository
    {
        public int AddUser(User user)
        {
            var connection = CreateConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();

            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = "INSERT INTO Users(email, nick, password) values(@email, @nick, @password)";
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@nick", user.Nick);
            command.Parameters.AddWithValue("@password", user.Password);

            int affectedRows;

            try {
                affectedRows = command.ExecuteNonQuery();
            } catch (Exception) {
                transaction.Rollback();
                connection.Close();
                return 0;
            }

            if (affectedRows != 1)
            {
                transaction.Rollback();
                connection.Close();
                return 0;
            }

            transaction.Commit();
            connection.Close();

            return affectedRows;
        }

        public User GetUserByNickName(string nick)
        {
            var connection = CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "select id_user, nick, email, password from Users where nick = @nick";
            command.Parameters.AddWithValue("@nick", nick);
            var reader = command.ExecuteReader();

            if (!reader.HasRows) throw new NoMatchingRecordsException();
            
            reader.Read();
            var user = new User()
            {
                IdUser = reader.GetInt32(0),
                Nick = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3)
            };

            connection.Close();

            return user;
        }
    }
}
