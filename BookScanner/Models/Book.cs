using SQLite;

namespace BookScanner.Models
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public string Notes { get; set; }
        public string Tags { get; set; }
        public string ISBN { get; set; }
    }
}
