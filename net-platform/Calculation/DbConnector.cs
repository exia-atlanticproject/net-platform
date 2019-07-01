using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace net_platform.Calculation
{
    public class DbConnector
    {
        private static bool _isCreated = false;
        private static MySqlConnection _mysqlClient;
        public static int _lastId;
        
        private static void InitializeClient()
        {
            string host = "localhost";
            string db = "calculationdb";
            string user = "calculationdb";
            string password = "p@ssw0rd";

            _mysqlClient = new MySqlConnection("SERVER=" + host + ";" + "DATABASE=" + db + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";");
            
            try
            {
                _mysqlClient.Open();
                _isCreated = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static MySqlConnection getDbClient()
        {
            if (!_isCreated)
            {
                InitializeClient();
                _isCreated = true;
            }
            return _mysqlClient;
        }

        public static void insertUser(string userId)
        {
            MySqlCommand cmd = getDbClient().CreateCommand();
            cmd.CommandText = "INSERT INTO Users (distantUser) VALUES(@userId)";
            cmd.Parameters.AddWithValue("@userID", userId);
            cmd.ExecuteNonQuery();
        }

        public static bool verifyUser(string userId)
        {
            MySqlCommand cmd = getDbClient().CreateCommand();
            cmd.CommandText = "SELECT Count(*) FROM Users WHERE distantUser = @userID";
            cmd.Parameters.AddWithValue("@userID", userId);
            int count = int.Parse(cmd.ExecuteScalar()+"");
            if (count > 0) return true;
            return false;
        }
        public static List<dynamic> getCalculList(int userId)
        {
            MySqlCommand cmd = getDbClient().CreateCommand();
            cmd.CommandText = "SELECT * FROM Jobs NATURAL JOIN Granularities NATURAL JOIN Types NATURAL JOIN Users WHERE Users.distantUser = @idUser";
            cmd.Parameters.AddWithValue("@idUser", userId);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            List<dynamic> calculList = new List<dynamic>();
            while (dataReader.Read())
            {
                calculList.Add(new
                {
                    nameJob=dataReader["nameJob"].ToString(),
                    createdJob=dataReader["createdJob"].ToString(),
                    startJob=dataReader["startJob"].ToString(),
                    endJob=dataReader["endJob"].ToString(),
                    idUser=dataReader["distantUser"].ToString(),
                    typeGranularity=dataReader["typeGranularity"].ToString(),
                    nameType=dataReader["nameType"].ToString()
                });
            }
            dataReader.Close();

            return calculList;
        }

        public static List<double> getCalculById(int jobId)
        {
            MySqlCommand cmd = getDbClient().CreateCommand();
            cmd.CommandText = "SELECT * FROM Results WHERE idJob = @idJob";
            cmd.Parameters.AddWithValue("@idJob", jobId);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            List<double> jobResults = new List<double>();
            while (dataReader.Read())
            {
                double result = double.Parse(dataReader["outputResult"].ToString());
                jobResults.Add(result);
            }
            dataReader.Close();

            return jobResults;
        }

        public static int createJob(string name, string start, string end, string userId, string granularity, string type)
        {
            MySqlCommand cmd = getDbClient().CreateCommand();
            cmd.CommandText = "INSERT INTO Jobs (nameJob, startJob, endJob, idUser, idGranularity, idType) VALUES (@name, @start, @end, (SELECT idUser FROM Users WHERE distantUser=@userId), (SELECT idGranularity FROM Granularities WHERE typeGranularity=@granularity), (SELECT idType FROM Types WHERE nameType=@type))";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@granularity", granularity);
            cmd.Parameters.AddWithValue("@type", type);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            _lastId = (int) cmd.LastInsertedId;

            return _lastId;
        }

        public static void addResult(int idJob, double result)
        {
            MySqlCommand cmd = getDbClient().CreateCommand();
            cmd.CommandText = "INSERT INTO Results (outputResult, idJob) VALUES(@result, @idJob)";
            cmd.Parameters.AddWithValue("@result", result);
            cmd.Parameters.AddWithValue("@idJob", idJob);
            cmd.ExecuteNonQuery();
        }
    }
}