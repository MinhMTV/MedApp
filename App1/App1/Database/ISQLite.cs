using SQLite;

namespace CBMTraining.Database
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
