using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    public class DbTmpl
    {
        public string ConnectionString { get; set; }
        public DbTmpl(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public List<T> ExecuteSelect<T>(string select, Dictionary<string, object> parameters , Func<SqlDataReader, T> mapper)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(select,conn))
                {
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> p in parameters)
                        {
                            cmd.Parameters.AddWithValue(p.Key, p.Value);
                        }
                    }
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<T> result = new List<T>();
                        while (reader.Read())
                        {
                            result.Add(mapper.Invoke(reader));
                        }
                        return result;
                    }
                }
            }
        }
        public int ExecuteStatement(string statement, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(statement, conn))
                {
                    foreach (KeyValuePair<string, object> p in parameters)
                    {
                        cmd.Parameters.AddWithValue(p.Key, p.Value);
                    }
                    int affectedRows = cmd.ExecuteNonQuery();
                    return affectedRows;
                }
            }
        }
        public T ExecuteStatementScalar<T>(string statement, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(statement, conn))
                {
                    foreach (KeyValuePair<string, object> p in parameters)
                    {
                        cmd.Parameters.AddWithValue(p.Key, p.Value);
                    }
                    T scalar = (T)cmd.ExecuteScalar();
                    return scalar;
                }
            }
        }
    }
}
