using DeskBooking.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeskBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeskController : ControllerBase
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

        // Return all desks
        [HttpGet("{Email}, {Password}")]
        public IEnumerable<Desk>? GetAll(string Email, string Password)
        {
            if (isSystemUser(Email, Password))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("SELECT * FROM Desk", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<Desk> desks = new List<Desk>();
                    while (reader.Read())
                    {
                        Desk desk = new Desk();
                        desk.DeskId = reader.GetInt32(0);
                        desk.LocationId = reader.GetInt32(1);
                        desk.Note = reader.GetString(2);
                        desks.Add(desk);
                    }
                    reader.Close();
                    connection.Close();
                    return desks;
                }
                else
                {
                    reader.Close();
                    connection.Close();
                    return new List<Desk>();
                }
            }
            else
            {
                return new List<Desk>();
            }
        }

        // Return available desks
        [HttpGet("{startDate}, {endDate}, {Email}, {Password}")]
        public IEnumerable<Desk>? GetAvailable(DateTime startDate, DateTime EndDate, string Email, string Password)
        {
            if (isSystemUser(Email, Password))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand($"SELECT * FROM Desk WHERE DeskId NOT IN (SELECT DeskId FROM Reservation WHERE StartDate <= {startDate} AND EndDate >= {EndDate})", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<Desk> desks = new List<Desk>();
                    while (reader.Read())
                    {
                        Desk desk = new Desk();
                        desk.DeskId = reader.GetInt32(0);
                        desk.LocationId = reader.GetInt32(1);
                        desk.Note = reader.GetString(2);
                        desks.Add(desk);
                    }
                    reader.Close();
                    connection.Close();
                    return desks;
                }
                else
                {
                    reader.Close();
                    connection.Close();
                    return new List<Desk>();
                }
            }
            else
            {
                return new List<Desk>();
            }
        }

        // Return desks in location
        [HttpGet("location/{locationId}, {Email}, {Password}")]
        public IEnumerable<Desk>? GetByLocation(int locationId, string Email, string Password)
        {
            if (isSystemUser(Email, Password))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand($"SELECT * FROM Desk WHERE LocationId = {locationId}", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<Desk> desks = new List<Desk>();
                    while (reader.Read())
                    {
                        Desk desk = new Desk();
                        desk.DeskId = reader.GetInt32(0);
                        desk.LocationId = reader.GetInt32(1);
                        desk.Note = reader.GetString(2);
                        desks.Add(desk);
                    }
                    reader.Close();
                    connection.Close();
                    return desks;
                }
                else
                {
                    reader.Close();
                    connection.Close();
                    return new List<Desk>();
                }
            }
            else
            {
                return new List<Desk>();
            }
        }

        // Return desk entry
        [HttpGet("{id}, {Email}, {Password}")]
        public Desk? GetById(int id, string Email, string Password)
        {
            if (isSystemUser(Email, Password))
            {
                Desk desk = new Desk();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Desk WHERE DeskId = {id}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                desk.DeskId = reader.GetInt32(0);
                                desk.LocationId = reader.GetInt32(1);
                                desk.Note = reader.GetString(2);
                            }
                        }
                    }
                }
                return desk;
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

        // Add desk entry
        [HttpPost("{desk}, {Email}, {Password}")]
        public void Post(Desk desk, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"INSERT INTO Desk (LocationId, Note) VALUES ('{desk.LocationId}', '{desk.Note}')";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // Update desk entry
        [HttpPut("{desk}, {Email}, {Password}")]
        public void Put(Desk desk, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"UPDATE Desk SET LocationId = '{desk.LocationId}', Note = '{desk.Note}' WHERE LocationId = {desk.DeskId}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

        }

        // Delete desk based on id if no reservations exist
        [HttpDelete("{id}, {Email}, {Password}")]
        public void Delete(int id, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM Reservation WHERE DeskId = {id}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                        {
                            query = $"DELETE FROM Desk WHERE DeskId = {id}";
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
