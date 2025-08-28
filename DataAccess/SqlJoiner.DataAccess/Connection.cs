using DatabaseSetting.Model;
using Npgsql;
using System.Configuration;
using System.Data;

namespace SqlJoiner.DataAccess
{
    public enum ConnectionEnum
    {
        POSTGRESQL,
        SQLSERVER
    }

    public static class Connection
    {
        public static IDbConnection? DbConnection { get; set; }
        public static string? ConnectionString
        {
            get
            {
                // set a custom path for your app.config
                NpgsqlConnectionStringBuilder cnBuilder = new NpgsqlConnectionStringBuilder();
                cnBuilder["Server"] = DataCommunication.ServerName;
                cnBuilder["Port"] = DataCommunication.PortNumber;
                cnBuilder["Database"] = DataCommunication.DatabaseName;
                cnBuilder["UserId"] = DataCommunication.UserId;
                cnBuilder["Password"] = DataCommunication.Password;

                return cnBuilder.ToString();
            }
        }
    }
}
