using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;

namespace Northwind.Logging
{
    public class HttpSqlLogger : IHttpLogger
    {
        private readonly SqlConnection _connection;

        public bool DebugEnabled { get; set; }

        public HttpSqlLogger() : this(ConfigurationManager.ConnectionStrings["LoggingConnection"].ToString())
        {

        }
        public HttpSqlLogger(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
        public void WriteDebug(string title, string message)
        {
            if (!DebugEnabled) return;
        }

        public void WriteError(string title, Exception exception)
        {
            throw new NotImplementedException();
        }

        public Task WriteHttpResponseAsync(IHttpLog log)
        {
            const string cmdText = "dbo.AddHttpLog";
            using (var cmd = new SqlCommand(cmdText, _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LogId", log.Id);
                cmd.Parameters.AddWithValue("@RequestDate", log.RequestDate);
                cmd.Parameters.AddWithValue("@RequestMethod", log.RequestMethod);
                cmd.Parameters.AddWithValue("@RequestUrl", log.RequestUrl);
                cmd.Parameters.AddWithValue("@HttpStatusCode", log.HttpStatusCode);
                cmd.Parameters.AddWithValue("@ThreadId", log.ThreadId);
                cmd.Parameters.AddWithValue("@RemoteAddress", log.RemoteAddress);
                cmd.Parameters.AddWithValue("@Username", log.UserName);
                cmd.Parameters.AddWithValue("@Message", log.Message);
                cmd.Parameters.AddWithValue("@ResponseTime", log.ResponseTime);
                cmd.Parameters.AddWithValue("@Resource", log.Resource);

                _connection.Open();
                return cmd.ExecuteNonQueryAsync();
            }
        }


        public void WriteInfo(string title, string message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}