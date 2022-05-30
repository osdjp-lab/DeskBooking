using DeskBooking.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeskBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
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

        // Return all reservations
        [HttpGet("{Email}, {Password}")]
        public IEnumerable<Reservation>? Get(string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("SELECT * FROM Reservation", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<Reservation> reservations = new List<Reservation>();
                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();
                        reservation.ReservationId = reader.GetInt32(0);
                        reservation.DeskId = reader.GetInt32(1);
                        reservation.UserId = reader.GetInt32(2);
                        reservation.StartDate = reader.GetDateTime(3);
                        reservation.EndDate = reader.GetDateTime(4);
                        reservations.Add(reservation);
                    }
                    reader.Close();
                    connection.Close();
                    return reservations;
                }
                else
                {
                    reader.Close();
                    connection.Close();
                    return new List<Reservation>();
                }
            }
            else if (isSystemUser(Email, Password))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand($"SELECT * FROM Reservation WHERE UserId = (SELECT UserId FROM SystemUser WHERE Email = {Email} AND Password = {Password})", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<Reservation> reservations = new List<Reservation>();
                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();
                        reservation.ReservationId = reader.GetInt32(0);
                        reservation.DeskId = reader.GetInt32(1);
                        reservation.UserId = reader.GetInt32(2);
                        reservation.StartDate = reader.GetDateTime(3);
                        reservation.EndDate = reader.GetDateTime(4);
                        reservations.Add(reservation);
                    }
                    reader.Close();
                    connection.Close();
                    return reservations;
                }
                else
                {
                    reader.Close();
                    connection.Close();
                    return new List<Reservation>();
                }
            }
            else
            {
                return new List<Reservation>();
            }
        }

        // Return user reservation by id
        [HttpGet("{id}, {Email}, {Password}")]
        public Reservation? Get(int id, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                Reservation reservation = new Reservation();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Reservation WHERE ReservationId = {id}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reservation.ReservationId = reader.GetInt32(0);
                                reservation.DeskId = reader.GetInt32(1);
                                reservation.UserId = reader.GetInt32(2);
                                reservation.StartDate = reader.GetDateTime(3);
                                reservation.EndDate = reader.GetDateTime(4);
                            }
                        }
                    }
                }
                return reservation;
            }
            else if (isSystemUser(Email, Password))
            {
                Reservation reservation = new Reservation();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Reservation WHERE UserId = (SELECT UserId FROM SystemUser WHERE Email = {Email} AND Password = {Password})";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reservation.ReservationId = reader.GetInt32(0);
                                reservation.DeskId = reader.GetInt32(1);
                                reservation.UserId = reader.GetInt32(2);
                                reservation.StartDate = reader.GetDateTime(3);
                                reservation.EndDate = reader.GetDateTime(4);
                            }
                        }
                    }
                }
                return reservation;
            }
            else
            {
                return null;
            }
        }

        // Add reservation
        [HttpPost("{reservation}, {Email}, {Password}")]
        public void Post(Reservation reservation, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Reservation WHERE DeskId = {reservation.DeskId} AND ((StartDate BETWEEN {reservation.StartDate} AND {reservation.EndDate}) OR (EndDate BETWEEN {reservation.StartDate} AND {reservation.EndDate}))";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Close();
                                connection.Close();
                                return;
                            }
                            else
                            {
                                reader.Close();
                                string query2 = $"INSERT INTO Reservation (DeskId, UserId, StartDate, EndDate) VALUES ({reservation.DeskId}, {reservation.UserId}, {reservation.StartDate}, {reservation.EndDate})";
                                using (SqlCommand command2 = new SqlCommand(query2, connection))
                                {
                                    command2.ExecuteNonQuery();
                                }
                                connection.Close();
                            }
                        }
                    }
                }
            }
            else if (isSystemUser(Email, Password))
            {
                if (reservation.EndDate - reservation.StartDate < new TimeSpan(7, 0, 0, 0))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = $"SELECT * FROM Reservation WHERE DeskId = {reservation.DeskId} AND ((StartDate BETWEEN {reservation.StartDate} AND {reservation.EndDate}) OR (EndDate BETWEEN {reservation.StartDate} AND {reservation.EndDate}))";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Close();
                                    connection.Close();
                                    return;
                                }
                                else
                                {
                                    reader.Close();
                                    string query2 = $"SELECT * FROM Reservation WHERE UserId = (SELECT UserId FROM SystemUser WHERE Email = {Email} AND Password = {Password})";
                                    using (SqlCommand command2 = new SqlCommand(query2, connection))
                                    {
                                        using (SqlDataReader reader2 = command2.ExecuteReader())
                                        {
                                            if (reader2.HasRows)
                                            {
                                                reader2.Close();
                                                string query3 = $"INSERT INTO Reservation (DeskId, UserId, StartDate, EndDate) VALUES ({reservation.DeskId}, {reservation.UserId}, {reservation.StartDate}, {reservation.EndDate})";
                                                using (SqlCommand command3 = new SqlCommand(query3, connection))
                                                {
                                                    command3.ExecuteNonQuery();
                                                }
                                                connection.Close();
                                            }
                                            else
                                            {
                                                reader2.Close();
                                                connection.Close();
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Update reservation
        [HttpPut("{reservation}, {Email}, {Password}")]
        public void Put(Reservation reservation, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Reservation WHERE DeskId = {reservation.DeskId} AND ((StartDate BETWEEN {reservation.StartDate} AND {reservation.EndDate}) OR (EndDate BETWEEN {reservation.StartDate} AND {reservation.EndDate}))";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Close();
                                connection.Close();
                                return;
                            }
                            else
                            {
                                reader.Close();
                                string query2 = $"UPDATE Reservation SET DeskId = {reservation.DeskId}, UserId = {reservation.UserId}, StartDate = {reservation.StartDate}, EndDate = {reservation.EndDate} WHERE ReservationId = {reservation.ReservationId}";
                                using (SqlCommand command2 = new SqlCommand(query2, connection))
                                {
                                    command2.ExecuteNonQuery();
                                }
                                connection.Close();
                            }
                        }
                    }
                }
            }
            else if (isSystemUser(Email, Password))
            {
                if (reservation.EndDate - reservation.StartDate < new TimeSpan(7, 0, 0, 0))
                {
                    DateTime currentDateTime = DateTime.Now;
                    if (currentDateTime.AddHours(24) > reservation.StartDate)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = $"SELECT * FROM Reservation WHERE DeskId = {reservation.DeskId} AND ((StartDate BETWEEN {reservation.StartDate} AND {reservation.EndDate}) OR (EndDate BETWEEN {reservation.StartDate} AND {reservation.EndDate}))";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Close();
                                        connection.Close();
                                        return;
                                    }
                                    else
                                    {
                                        reader.Close();
                                        string query2 = $"SELECT * FROM Reservation WHERE UserId = (SELECT UserId FROM SystemUser WHERE Email = {Email} AND Password = {Password})";
                                        using (SqlCommand command2 = new SqlCommand(query2, connection))
                                        {
                                            using (SqlDataReader reader2 = command2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    reader2.Close();
                                                    string query3 = $"UPDATE Reservation SET DeskId = {reservation.DeskId}, UserId = {reservation.UserId}, StartDate = {reservation.StartDate}, EndDate = {reservation.EndDate} WHERE ReservationId = {reservation.ReservationId}";
                                                    using (SqlCommand command3 = new SqlCommand(query3, connection))
                                                    {
                                                        command3.ExecuteNonQuery();
                                                    }
                                                    connection.Close();
                                                }
                                                else
                                                {
                                                    reader2.Close();
                                                    connection.Close();
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }

                }
            }
        }

        // Delete reservation based on Id
        [HttpDelete("{id}, {Email}, {Password}")]
        public void Delete(int id, string Email, string Password)
        {
            if (isAdmin(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"DELETE FROM Reservation WHERE ReservationId = {id}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            else if (isSystemUser(Email, Password))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Reservation WHERE UserId = (SELECT UserId FROM SystemUser WHERE Email = {Email} AND Password = {Password})";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Close();
                                
                                string query2 = $"DELETE FROM Reservation WHERE ReservationId = {id}";
                                using (SqlCommand command2 = new SqlCommand(query2, connection))
                                {
                                    command2.ExecuteNonQuery();
                                }
                                connection.Close();
                            }
                            else
                            {
                                reader.Close();
                                connection.Close();
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
