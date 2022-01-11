using SQLite;

namespace App1.Models
{
    class Pictures
    {
        [PrimaryKey]
        public int TypeId { get; set; }

        public PicType Type { get; set; }

        public string Photo { get; set; }

    }
}
