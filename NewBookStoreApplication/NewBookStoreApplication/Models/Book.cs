using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewBookStoreApplication.Models
{
    [Serializable]
    public class Book
    {
       
        
            public int BookId { get; set; }  // 0 = Insert, >0 = Update
            public string Title { get; set; }
            public string Author { get; set; }
            public string ISBN { get; set; }
            public decimal Price { get; set; }
            public DateTime PublishedDate { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
            public int StockQuantity { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
       }
    
}