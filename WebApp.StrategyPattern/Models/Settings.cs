namespace WebApp.StrategyPattern.Models
{
    public class Settings
    {
        public static string ClaimdatabaseType = "databasetype";
        public EDatabaseType DatabaseType;
        public EDatabaseType GetDefaultDatabaseType => EDatabaseType.SqlServer;

    }
}
