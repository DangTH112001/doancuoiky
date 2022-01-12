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
                    Console.WriteLine(ex);
                    return 0;
                }
                conn.Close();
            }
            return 0; // Không có user trong database
        }

        public List<Object> getUser(int id) {
            List<Object> result = new List<Object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try {
                    string query = "SELECT A.name name, A.SLTG SLTG, B.SLT SLT, get_rank(@id0) r" +
                                    " FROM (" +
                                    " (" +
                                        " SELECT user.id, name, count(interaction.mcid) SLTG " +
                                        " from user inner join interaction on user.id = interaction.uid" +
                                        " where user.id = @id1" +
                                        " group by user.id, user.name " +  
                                    " ) A," +
                                    " (" +
                                        " SELECT count(multiplechoice.id) SLT " +
                                        " from user inner join multiplechoice on user.id = multiplechoice.uid" +
                                        " where user.id = @id2" +                                        
                                        " group by user.id, user.name " +

                                    " ) B" +
                                    " )" +
                                    " WHERE A.id = @id3";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id0", id);
                    cmd.Parameters.AddWithValue("id1", id);
                    cmd.Parameters.AddWithValue("id2", id);
                    cmd.Parameters.AddWithValue("id3", id);
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read())
                        {
                            var obj = new
                            {
                                name = reader["name"].ToString(),
                                SLTG = Convert.ToInt32(reader["SLTG"]),
                                SLT = Convert.ToInt32(reader["SLT"]),
                                rank = Convert.ToInt32(reader["r"])
                            };
                            result.Add(obj);
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return null;
                }
                conn.Close();
            }
            return result;
        }

        public int xoaQuestion(int qid) {
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                try {
                    string query = "DELETE FROM question WHERE id = @qid";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("qid", qid);
                    return (cmd.ExecuteNonQuery());

                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return -1;
                }
            }
        }

        public int xoaBelong(int qid, int mcid) {
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                try {
                    string query = "DELETE FROM belong WHERE qid = @qid AND mcid = @mcid";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("qid", qid);
                    cmd.Parameters.AddWithValue("mcid", mcid);
                    return (cmd.ExecuteNonQuery());

                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return -1;
                }
            }
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
                    Console.WriteLine(ex);
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
                    Console.WriteLine(ex);
                    return 0;
                }
                conn.Close();
            }
            return 0; // Không có user trong database
        }

        public List<Question> getQuestions(int uid) {
            List<Question> result = new List<Question>();
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                string query = "SELECT * FROM QUESTION WHERE uid = @uid";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("uid", uid);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        result.Add(new Question()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            question = reader["question"].ToString(),
                            filter = reader["filter"].ToString(),
                            A = reader["A"].ToString(),
                            B = reader["B"].ToString(),
                            C = reader["C"].ToString(),
                            D = reader["D"].ToString(),
                            answer = reader["answer"].ToString(),
                            uid = Convert.ToInt32(reader["uid"])
                        }); ;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return result;
        }

        public List<Question> getQuestionsfromQuiz(int mcid) {
            List<Question> result = new List<Question>();
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                string query = "SELECT * FROM QUESTION INNER JOIN BELONG ON QUESTION.id = BELONG.qid WHERE BELONG.mcid = @mcid";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("mcid", mcid);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        result.Add(new Question()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            question = reader["question"].ToString(),
                            filter = reader["filter"].ToString(),
                            A = reader["A"].ToString(),
                            B = reader["B"].ToString(),
                            C = reader["C"].ToString(),
                            D = reader["D"].ToString(),
                            answer = reader["answer"].ToString(),
                            uid = Convert.ToInt32(reader["uid"])
                        }); ;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return result;
        }

        public List<Question> getQuestion(int qid) {
            List<Question> result = new List<Question>();
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                string query = "SELECT * FROM QUESTION WHERE id = @qid";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("qid", qid);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        result.Add(new Question()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            question = reader["question"].ToString(),
                            filter = reader["filter"].ToString(),
                            A = reader["A"].ToString(),
                            B = reader["B"].ToString(),
                            C = reader["C"].ToString(),
                            D = reader["D"].ToString(),
                            answer = reader["answer"].ToString(),
                            uid = Convert.ToInt32(reader["uid"])
                        }); ;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return result;
        }

        public List<Question> getQuestions(string filter) {
            List<Question> result = new List<Question>();
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                string query = "SELECT * FROM QUESTION WHERE filter = @filter";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("filter", filter);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        result.Add(new Question()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            question = reader["question"].ToString(),
                            filter = reader["filter"].ToString(),
                            A = reader["A"].ToString(),
                            B = reader["B"].ToString(),
                            C = reader["C"].ToString(),
                            D = reader["D"].ToString(),
                            answer = reader["answer"].ToString(),
                            uid = Convert.ToInt32(reader["uid"])
                        }); ;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return result;
        }

        public List<Question> getQuestions(string[] list) {
            List<Question> result = new List<Question>();
            string ids = "";
            for (int i = 0; i < list.Length; i++) {
                ids += list[i] + ",";
            }
            ids = ids.Substring(0, ids.Length-1);
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                string query = "SELECT * FROM QUESTION WHERE id in (" + ids + ")";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        result.Add(new Question()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            question = reader["question"].ToString(),
                            filter = reader["filter"].ToString(),
                            A = reader["A"].ToString(),
                            B = reader["B"].ToString(),
                            C = reader["C"].ToString(),
                            D = reader["D"].ToString(),
                            answer = reader["answer"].ToString(),
                            uid = Convert.ToInt32(reader["uid"])
                        }); ;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return result;
        }
        
        public int themQuestion(string question, string filter, string A, string B, string C, string D, string answer, int uid) {
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                try {
                    string query = "INSERT INTO QUESTION(question, filter, A, B, C, D, answer, uid) VALUES(@question, @filter, @A, @B, @C, @D, @answer, @uid)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("question", question);
                    cmd.Parameters.AddWithValue("filter", filter);
                    cmd.Parameters.AddWithValue("A", A);
                    cmd.Parameters.AddWithValue("B", B);
                    cmd.Parameters.AddWithValue("C", C);
                    cmd.Parameters.AddWithValue("D", D);
                    cmd.Parameters.AddWithValue("answer", answer);
                    cmd.Parameters.AddWithValue("uid", uid);
                    return (cmd.ExecuteNonQuery());
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return -1;
                }
                
            }
        }
        public int capnhatQuestion(int id, string question, string filter, string A, string B, string C, string D, string answer) {
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                try {
                    string query = "UPDATE QUESTION SET question = @question, filter = @filter, A = @A, B = @B, C = @C, D = @D, answer =  @answer WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("question", question);
                    cmd.Parameters.AddWithValue("filter", filter);
                    cmd.Parameters.AddWithValue("A", A);
                    cmd.Parameters.AddWithValue("B", B);
                    cmd.Parameters.AddWithValue("C", C);
                    cmd.Parameters.AddWithValue("D", D);
                    cmd.Parameters.AddWithValue("answer", answer);
                    cmd.Parameters.AddWithValue("id", id);
                    return (cmd.ExecuteNonQuery());
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return -1;
                }
                
            }
        }

        public int capnhatQuiz(int id, string title, string description, int time) {
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                try {
                    string query = "UPDATE multiplechoice SET title = @title, description = @description, time = @time WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("title", title);
                    cmd.Parameters.AddWithValue("description", description);
                    cmd.Parameters.AddWithValue("time", time);
                    cmd.Parameters.AddWithValue("id", id);
                    return (cmd.ExecuteNonQuery());
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return -1;
                }
                
            }
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

        public int addQuiz(string title, string description, int time, int uid) {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try {
                    string query = "INSERT INTO MULTIPLECHOICE(title, description, time, uid) VALUES (@title, @description, @time, @uid)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("title", title);
                    cmd.Parameters.AddWithValue("description", description);
                    cmd.Parameters.AddWithValue("time", time);
                    cmd.Parameters.AddWithValue("uid", uid);
                    int code = cmd.ExecuteNonQuery();

                    string query2 = "SELECT max(id) id FROM MULTIPLECHOICE";
                    MySqlCommand cmd2 = new MySqlCommand(query2, conn);
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
                    return -1;
                }
                
            }
        }

        public int addBelong(int[] qids, int mcid) {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try {
                    foreach (var qid in qids) {
                        string query = "INSERT INTO BELONG VALUES (@qid, @mcid)";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("qid", qid);
                        cmd.Parameters.AddWithValue("mcid", mcid);
                        cmd.ExecuteNonQuery();
                    }
                    return 0;
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return -1;
                }
            }
        }

        public List<Multiplechoice> getMultiplechoices(int uid) {
            List<Multiplechoice> result = new List<Multiplechoice>();
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                string query = "SELECT * FROM multiplechoice WHERE uid = @uid";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("uid", uid);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        result.Add(new Multiplechoice()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            title = reader["title"].ToString(),
                            description = reader["description"].ToString(),
                            time = Convert.ToInt32(reader["time"]),
                            total = Convert.ToInt32(reader["total"]),
                            participant = Convert.ToInt32(reader["participant"]),
                            uid = Convert.ToInt32(reader["uid"]),
                        }); ;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return result;
        }

        public List<Multiplechoice> getMultiplechoice(int mcid) {
            List<Multiplechoice> result = new List<Multiplechoice>();
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                string query = "SELECT * FROM multiplechoice WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("id", mcid);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        result.Add(new Multiplechoice()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            title = reader["title"].ToString(),
                            description = reader["description"].ToString(),
                            time = Convert.ToInt32(reader["time"]),
                            total = Convert.ToInt32(reader["total"]),
                            participant = Convert.ToInt32(reader["participant"]),
                            uid = Convert.ToInt32(reader["uid"]),
                        }); ;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return result;
        }

        public int xoaQuiz(int mcid) {
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                try {
                    string query1 = "DELETE FROM belong WHERE mcid = @mcid";
                    MySqlCommand cmd1 = new MySqlCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("mcid", mcid);
                    int code = cmd1.ExecuteNonQuery();

                    string query = "DELETE FROM multiplechoice WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id", mcid);
                
                    return (cmd.ExecuteNonQuery());

                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return -1;
                }
            }
        }
    }
}