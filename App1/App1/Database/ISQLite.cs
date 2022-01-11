using SQLite;

namespace App1.Database
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
