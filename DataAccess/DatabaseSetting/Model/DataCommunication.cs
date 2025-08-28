using System.Data;

namespace DatabaseSetting.Model
{
    public sealed class DataCommunication
    {
        public static IDbConnection? DatabaseConnection { get; set; }
        public static string? ServerName { get; set; }
        public static string? PortNumber { get; set; }
        public static string? DatabaseName { get; set; }
        public static string? UserId { get; set; }
        public static string? Password { get; set; }
    }
}
