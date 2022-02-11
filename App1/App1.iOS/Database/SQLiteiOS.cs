using CBMTraining.Database;
using CBMTraining.iOS.Database;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteiOS))]
namespace CBMTraining.iOS.Database
{
    public class SQLiteiOS : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            string sqliteFilename = "database.sqlite";
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentPath, "..", "Library", "Databases");
            var dbPathName = Path.Combine(libraryPath, sqliteFilename);
            SQLiteConnection conn = new SQLiteConnection(dbPathName);
            return conn;
        }
    }
}
