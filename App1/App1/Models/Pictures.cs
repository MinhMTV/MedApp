using SQLite;

namespace App1.Models
{
    public class Pictures
    {
        [PrimaryKey, AutoIncrement]
        public int TypeId { get; set; }

        public byte[] Image { get; set; } //Unique Imaque 
        public string ImagePath { get; set; } //ImagePath is unique //obsolete because only works with android File System, but very hard in IOS to implement
        public PicType Type { get; set; }

        public string Photo { get; set; } //image name from embedded images in app

    }
}
