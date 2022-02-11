using CBMTraining.Database;
using CBMTraining.Droid.Database;
using SQLite;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteAndroid))]
namespace CBMTraining.Droid.Database
{
    public class SQLiteAndroid : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            string sqliteFilename = "database.sqlite";
            string directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPathName = Path.Combine(directoryPath, sqliteFilename);
            SQLiteConnection conn = new SQLiteConnection(dbPathName);
            return conn;
        }
    }
}