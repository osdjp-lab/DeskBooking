using DeskBooking.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeskBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=DeskBooking.Data;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private Boolean isAdmin(string Email, string Password)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($"SELECT * FROM SystemUser WHERE Email = {Email} AND Password = {Password}", connection);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Password", Password);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                if (reader.GetBoolean(4))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Return all users
        [HttpGet]
        public IEnumerable<SystemUser>? Get(string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("SELECT * FROM SystemUser", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<SystemUser> users = new List<SystemUser>();
                    while (reader.Read())
                    {
                        SystemUser user = new SystemUser();
                        user.SystemUserId = reader.GetInt32(0);
                        user.Name = reader.GetString(1);
                        user.Surname = reader.GetString(2);
                        user.Email = reader.GetString(3);
                        user.Password = reader.GetString(4);
                        user.IsAdmin = reader.GetBoolean(5);
                        users.Add(user);
                    }
                    reader.Close();
                    connection.Close();
                    return users;
                }
                else
                {
                    reader.Close();
                    connection.Close();
                    return new List<SystemUser>();
                }
            }
            else
            {
                return new List<SystemUser>();
            }
        }

        // Return user by id
        [HttpGet("{id}, {Email}, {Password}")]
        public SystemUser? Get(int id, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                SystemUser user = new SystemUser();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM SystemUser WHERE SystemUserId = {id}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user.SystemUserId = reader.GetInt32(0);
                                user.Name = reader.GetString(1);
                                user.Surname = reader.GetString(2);
                                user.Email = reader.GetString(3);
                                user.Password = reader.GetString(4);
                                user.IsAdmin = reader.GetBoolean(5);
                            }
                        }
                    }
                }
                return user;
            }
            else
            {
                return null;
            }
        }

        // Return Users who reserved desks in LocationId
        [HttpGet("{id}, {Email}, {Password}")]
        public IEnumerable<SystemUser>? Get(int id, string Email, string Password, int LocationId)
        {
            if (isAdmin(Email, Password))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand($"SELECT * FROM SystemUser WHERE SystemUserId IN (SELECT SystemUserId FROM Desk WHERE LocationId = {LocationId})", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<SystemUser> users = new List<SystemUser>();
                    while (reader.Read())
                    {
                        SystemUser user = new SystemUser();
                        user.SystemUserId = reader.GetInt32(0);
                        user.Name = reader.GetString(1);
                        user.Surname = reader.GetString(2);
                        user.Email = reader.GetString(3);
                        user.Password = reader.GetString(4);
                        user.IsAdmin = reader.GetBoolean(5);
                        users.Add(user);
                    }
                    reader.Close();
                    connection.Close();
                    return users;
                }
                else
                {
                    reader.Close();
                    connection.Close();
                    return new List<SystemUser>();
                }
            }
            else
            {
                return new List<SystemUser>();
            }
        }

        // Add user
        [HttpPost("{user}, {Email}, {Password}")]
        public void Post(SystemUser user, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"INSERT INTO SystemUser (Name, Surname, Email, Password, IsAdmin) VALUES ('{user.Name}', '{user.Surname}', '{user.Email}', '{user.Password}', '{user.IsAdmin}')";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // Update user entry
        [HttpPut("{id}, {Email}, {Password}")]
        public void Put(SystemUser user, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"UPDATE SystemUser SET Name = '{user.Name}', Surname = '{user.Surname}', Email = '{user.Email}', Password = '{user.Password}', IsAdmin = '{user.IsAdmin}' WHERE SystemUserId = {user.SystemUserId}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // Delete user based on id if no reservations exist
        [HttpDelete("{id}, {Email}, {Password}")]
        public void Delete(int id, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM Reservation WHERE UserId = {id}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                        {
                            query = $"DELETE FROM SystemUser WHERE SystemUserId = {id}";
                            using (SqlCommand command2 = new SqlCommand(query, connection))
                            {
                                command2.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }
}
