using App1.Database;
using App1.Droid.Database;
using SQLite;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteAndroid))]
namespace App1.Droid.Database
{
    public class SQLiteAndroid : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            string sqliteFilename = "patient.sqlite";
            string directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPathName = Path.Combine(directoryPath, sqliteFilename);
            SQLiteConnection conn = new SQLiteConnection(dbPathName);
            return conn;
        }
    }
}