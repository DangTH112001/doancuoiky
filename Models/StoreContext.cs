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

        public int existUser(string username, string password) {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try {
                    string query = "SELECT COUNT(*) CNT FROM USER WHERE account=@username AND password=@password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);
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
                    string query = "INSERT INTO USER(account, password, name) VALUES (@username, @password, @fullname)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Parameters.AddWithValue("fullname", fullname);
                    int code = cmd.ExecuteNonQuery();
                    return 0;
                }
                catch (Exception ex) {
                    return -5;
                }
                
            }
        }
    }
}