using SQLite;

namespace App1.Models
{
    public class Pictures
    {
        [PrimaryKey]
        public int TypeId { get; set; }

        public PicType Type { get; set; }

        public string Photo { get; set; }

    }
}
