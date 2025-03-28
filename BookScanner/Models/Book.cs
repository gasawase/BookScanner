using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookScanner.Models
{
    internal class Book
    {
        [Key]
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public string Notes { get; set; }
        public string[] Tags { get; set; }
        public string ISBN { get; set; }
    }
}
