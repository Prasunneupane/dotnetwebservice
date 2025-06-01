using System;
using System.ComponentModel.DataAnnotations;

namespace BookstoreLibrary.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Author { get; set; }

        [StringLength(20)]
        public string ISBN { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public DateTime? PublishedDate { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}