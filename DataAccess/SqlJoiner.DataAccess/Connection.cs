using System.Configuration;
using System.Data;

namespace SqlJoiner.DataAccess
{
    public static class Connection
    {
        public static IDbConnection? DbConnection { get; set; }
        public static string? ConnectionString
        {
            get
            {
                // set a custom path for your app.config
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "App.Config");
                return ConfigurationManager.ConnectionStrings["postgres"]?.ConnectionString;
            }
        }
    }
}
