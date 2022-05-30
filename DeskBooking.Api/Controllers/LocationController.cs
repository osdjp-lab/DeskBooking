using DeskBooking.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeskBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=DeskBooking.Data;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private Boolean isSystemUser(string Email, string Password)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($"SELECT * FROM SystemUser WHERE Email = {Email} AND Password = {Password}", connection);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Password", Password);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: api/<LocationController>
        // Return all locations
        [HttpGet("{Email}, {Password}")]
        public IEnumerable<Location>? Get(string Email, string Password)
        {
            if (isSystemUser(Email, Password))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("SELECT * FROM Location", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<Location> locations = new List<Location>();
                    while (reader.Read())
                    {
                        Location location = new Location();
                        location.LocationId = reader.GetInt32(0);
                        location.Country = reader.GetString(1);
                        location.City = reader.GetString(2);
                        location.Street = reader.GetString(3);
                        locations.Add(location);
                    }
                    reader.Close();
                    connection.Close();
                    return locations;
                }
                else
                {
                    reader.Close();
                    connection.Close();
                    return new List<Location>();
                }
            }
            else
            {
                return new List<Location>();
            }
        }

        // GET api/<LocationController>/5
        // Return location by id
        [HttpGet("{id}, {Email}, {Password}")]
        public Location? Get(int id, string Email, string Password)
        {
            if (isSystemUser(Email, Password))
            {
                Location location = new Location();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Location WHERE LocationId = {id}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                location.LocationId = reader.GetInt32(0);
                                location.Country = reader.GetString(1);
                                location.City = reader.GetString(2);
                                location.Street = reader.GetString(3);
                            }
                        }
                    }
                }
                return location;
            }
            else
            {
                return null;
            }
        }

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

        // POST api/<LocationController>
        // Add location entry
        [HttpPost("{location}, {Email}, {Password}")]
        public void Post(Location location, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"INSERT INTO Location (Country, City, Street) VALUES ('{location.Country}', '{location.City}', '{location.Street}')";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // PUT api/<LocationController>/5
        // Update a location based on the id
        [HttpPut("{location}, {Email}, {Password}")]
        public void Put(Location location, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"UPDATE Location SET Country = '{location.Country}', City = '{location.City}', Street = '{location.Street}' WHERE LocationId = {location.LocationId}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            
        }

        // DELETE api/<LocationController>/5
        // Delete location based on id if no desk is assigned to it
        [HttpDelete("{id}, {Email}, {Password}")]
        public void Delete(int id, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM Desk WHERE LocationId = {id}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                        {
                            query = $"DELETE FROM Location WHERE LocationId = {id}";
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
