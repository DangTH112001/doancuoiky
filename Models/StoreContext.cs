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

        public bool existUser(string username, string password) {
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
                                return true; // Có user trong database
                        reader.Close();
                    }
                }
                catch (Exception ex) {
                    return false;
                }
                conn.Close();
            }
            return false; // Không có user trong database
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
                    return (cmd.ExecuteNonQuery());
                }
                catch (Exception ex) {
                    return -1;
                }
                
            }
        }
    }
}