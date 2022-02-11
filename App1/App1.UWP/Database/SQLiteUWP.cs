using CBMTraining.Database;
using CBMTraining.UWP.Database;
using SQLite;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteUWP))]
namespace CBMTraining.UWP.Database
{
    public class SQLiteUWP : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "database.sqlite";
            var path = Path.Combine(ApplicationData.
              Current.LocalFolder.Path, dbName);
            return new SQLiteConnection(path);
        }
    }
}
