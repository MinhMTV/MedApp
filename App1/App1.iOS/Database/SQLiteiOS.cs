using App1.Database;
using App1.iOS.Database;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteiOS))]
namespace App1.iOS.Database
{
    public class SQLiteiOS : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            string sqliteFilename = "patient.sqlite";
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentPath, "..", "Library", "Databases");
            var dbPathName = Path.Combine(libraryPath, sqliteFilename);
            SQLiteConnection conn = new SQLiteConnection(dbPathName);
            return conn;
        }
    }
}
