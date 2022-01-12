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

        public List<Question> getQuestions(string filter, int uid) {
            List<Question> result = new List<Question>();
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                string query = "SELECT * FROM QUESTION WHERE filter = @filter AND uid =@uid";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("filter", filter);
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

        public List<Multiplechoice> getBookmark(int uid) {
            List<Multiplechoice> result = new List<Multiplechoice>();
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                string query = "SELECT m.id, m.title, m.description, m.total, m.time, m.participant FROM multiplechoice m inner join interaction i on m.id = i.mcid  WHERE i.uid = @uid AND i.favorite = 1";
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
                            uid = uid,
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

        public int xoaBookmark(int uid, int mcid) {
            using (MySqlConnection conn = GetConnection()) {
                conn.Open();
                try {
                    string query1 = "UPDATE interaction SET favorite = 0 WHERE uid = @uid and mcid = @mcid";
                    MySqlCommand cmd1 = new MySqlCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("uid", uid);
                    cmd1.Parameters.AddWithValue("mcid", mcid);
                    int code = cmd1.ExecuteNonQuery();
                
                    return (cmd1.ExecuteNonQuery());
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    return -1;
                }
            }
        }

        // Hoài Nhân
        public List<Multiplechoice> FetchMultiplechoicesToHome(int limit, int start)
        {
            List<Multiplechoice> list = new List<Multiplechoice>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = "select id, description, title, total, time, participant from multiplechoice limit @start, @limit";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("limit", limit);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        list.Add(new Multiplechoice()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            description = reader["description"].ToString(),
                            title = reader["title"].ToString(),
                            total = Convert.ToInt32(reader["total"]),
                            time = Convert.ToInt32(reader["time"]),
                            participant = Convert.ToInt32(reader["participant"])
                        });
                    }
                    reader.Close();
                }

                conn.Close();
            }

            return list;
        }

        public List<Multiplechoice> FindQuiz(string filter, string text)
        {
            List<Multiplechoice> list = new List<Multiplechoice>();

            using (MySqlConnection conn = GetConnection())
            {
                string query = "";

                conn.Open();

                if (filter == "all")
                {
                    query = @"
                        select id, title, description, time, total, participant 
                        from multiplechoice 
                        where lower(title) like '%" + text + @"%'
                        union 
                        select id, title, description, time, total, participant 
                        from multiplechoice 
                        where lower(description) like '%" + text + "%'";
                }
                else
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        query = @"
                            select id, title, description, time, total, participant 
                            from multiplechoice 
                            where filter = '" + filter + "'";
                    }
                    else
                    {
                        query = @"
                            select id, title, description, time, total, participant
                                from multiplechoice 
                                where lower(title) like '%" + text + @"%' and filter = '" + filter + @"'
                            union 
                            select id, title, description, time, total, participant
                                from multiplechoice 
                                where lower(description) like '%" + text + @"%' and filter = '" + filter + "'";
                    }
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            list.Add(new Multiplechoice()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                description = reader["description"].ToString(),
                                title = reader["title"].ToString(),
                                total = Convert.ToInt32(reader["total"]),
                                time = Convert.ToInt32(reader["time"]),
                                participant = Convert.ToInt32(reader["participant"])
                            });
                        }
                        reader.Close();
                    }
                }

                conn.Close();
            }

            return list;
        }

        public List<object> GetQuiz(int QuizID)
        {
            List<object> list = new List<object>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                        select q.id QuestionID, q.question QuestionContent, 
                            q.a OptA, q.b OptB, q.c OptC, q.d OptD, 
                            m.time Time
                        from multiplechoice m, belong b, question q 
                        where m.id = b.mcid and b.qid = q.id and m.id = " + QuizID;

                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new
                            {
                                QuestionID = Convert.ToInt32(reader["QuestionID"]),
                                QuestionContent = reader["QuestionContent"].ToString(),
                                OptA = reader["OptA"],
                                OptB = reader["OptB"],
                                OptC = reader["OptC"],
                                OptD = reader["OptD"],
                                Time = Convert.ToInt32(reader["Time"])
                            };

                            list.Add(obj);
                        }

                        reader.Close();
                    }
                }

                conn.Close();
            }

            return list;
        }

        public List<AnswerList> checkResult(int QuizID)
        {
            List<AnswerList> list = new List<AnswerList>();
            string query = @"
            select q.id, q.answer 
            from question q, multiplechoice m, belong b
            where q.id = b.qid and b.mcid = m.id and m.id = " + QuizID;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            list.Add(new AnswerList()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                opt = reader["answer"].ToString()
                            });
                        }
                        reader.Close();
                    }
                }
                conn.Close();
            }
            return list;
        }

        public List<Question> GetResultByQuizID(int quizID)
        {
            List<Question> list = new List<Question>();
            string query = @"
                select q.id QuestionID, q.question QuestionContent, q.a OptA, q.b OptB, q.c OptC, q.d OptD, q.answer Answer
                from multiplechoice m, belong b, question q 
                where m.id = b.mcid and b.qid = q.id and m.id = " + quizID;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            list.Add(new Question()
                            {
                                id = Convert.ToInt32(reader["QuestionID"]),
                                question = reader["QuestionContent"].ToString(),
                                A = reader["OptA"].ToString(),
                                B = reader["OptB"].ToString(),
                                C = reader["OptC"].ToString(),
                                D = reader["OptD"].ToString(),
                                answer = reader["Answer"].ToString()
                            });
                        }
                        reader.Close();
                    }
                }
                conn.Close();
            }
            return list;
        }

        public int IsExistsBookmark(int quizID, int userID)
        {
            int exists = 0;
            string query = @"
                select count(*) count
                from interaction 
                where uid = " + userID + " and mcid = " + quizID;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            exists = Convert.ToInt32(reader["count"]);
                        }
                    }
                    reader.Close();
                }

                if (exists != 0)
                {
                    string query2 = @" 
                        select favorite
                        from interaction 
                        where uid = " + userID + " and mcid = " + quizID;

                    MySqlCommand cmd2 = new MySqlCommand(query2, conn);
                    using (var reader = cmd2.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                exists = Convert.ToInt32(reader["favorite"]) + 1;
                            }
                        }
                        reader.Close();
                    }
                }

                conn.Close();
            }

            // trả về 1 nếu đã bookmark, 0 nếu chưa
            return exists;
        }

        public int BookmarkQuiz(int quizID, int userID)
        {
            string query = "insert into interaction(uid, mcid, favorite) values('" + userID + "','" + quizID + "', 1)";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    return -1;
                }

                conn.Close();
            }

            return 0;
        }
        public int BookmarkQuizByUpdate(int quizID, int userID)
        {
            Console.WriteLine(quizID + " " + userID);
            string query = "update interaction set favorite = 1 where uid = @uid and mcid = @mcid";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("uid", userID);
                cmd.Parameters.AddWithValue("mcid", quizID);

                cmd.ExecuteNonQuery();

                conn.Close();
            }

            return 0;
        }

        public int addInteraction(int uid, int mcid, double score) {
            int exists = 0;
            int done = 0;
            string query = @"
                select count(*) cnt, done
                from interaction 
                where uid = " + uid + " and mcid = " + mcid;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            exists = Convert.ToInt32(reader["cnt"]);
                        }
                    }
                    reader.Close();
                }

                if (exists == 0)
                {
                    // Insert
                    string query1 = "insert into interaction(uid, mcid, done, score) values(@uid, @mcid, 1, @score)";
                    MySqlCommand cmd1 = new MySqlCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("uid", uid);
                    cmd1.Parameters.AddWithValue("mcid", mcid);
                    cmd1.Parameters.AddWithValue("score", score);

                    cmd1.ExecuteNonQuery();
                    
                }
                else if (done == 0) {
                    // Update
                    string query2 = "update interaction set done = 1, score = @score where uid = @uid and mcid = @mcid";
                    MySqlCommand cmd2 = new MySqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("score", score);
                    cmd2.Parameters.AddWithValue("uid", uid);
                    cmd2.Parameters.AddWithValue("mcid", mcid);

                    cmd2.ExecuteNonQuery();
                }

                conn.Close();
            }

            return 0;
        }
    }
}