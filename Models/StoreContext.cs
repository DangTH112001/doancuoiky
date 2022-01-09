using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doancuoiky.Models
{
    public class StoreContext
    {
        public string ConnectionString { get; set; } 

        public StoreContext(string connectionString) 
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection() 
        {
            return new MySqlConnection(ConnectionString);
        }

        public int getID(string username, string password) {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try {
                    string query = "SELECT COUNT(*) CNT, id FROM USER WHERE account=@username AND password=@password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) 
                            if (Convert.ToInt32(reader["CNT"]) == 1)
                                return Convert.ToInt32(reader["id"]); // Có user trong database
                        reader.Close();
                    }
                }
                catch (Exception ex) {
                    return 0;
                }
                conn.Close();
            }
            return 0; // Không có user trong database
        }

        public string getName(int id) {
            if (id <= 0) return null;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try {
                    string query = "SELECT name FROM USER WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            return reader["name"].ToString();
                        }
                    }
                }
                catch (Exception ex) {
                    return null;
                }
            }
            return null;
        }

        public int existUser(string username) {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try {
                    string query = "SELECT COUNT(*) CNT FROM USER WHERE account=@username";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) 
                            if (Convert.ToInt32(reader["CNT"]) == 1)
                                return 1; // Có user trong database
                        reader.Close();
                    }
                }
                catch (Exception ex) {
                    return 0;
                }
                conn.Close();
            }
            return 0; // Không có user trong database
        }

        public int addUser(string username, string password, string fullname) {
            if (username == null || password == null || fullname == null) return -2;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try {
                    string query1 = "INSERT INTO USER(account, password, name) VALUES (@username, @password, @fullname)";
                    MySqlCommand cmd1 = new MySqlCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("username", username);
                    cmd1.Parameters.AddWithValue("password", password);
                    cmd1.Parameters.AddWithValue("fullname", fullname);
                    int code = cmd1.ExecuteNonQuery();

                    string query2 = "SELECT id FROM USER WHERE account = @username";
                    MySqlCommand cmd2 = new MySqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("username", username);
                    using (var reader = cmd2.ExecuteReader())
                    {
                        while (reader.Read()) 
                            return Convert.ToInt32(reader["id"]); // Có user trong database
                        reader.Close();
                    }
                    return 0;
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return -5;
                }
                
            }
        }
    }
}