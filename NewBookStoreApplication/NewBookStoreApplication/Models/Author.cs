using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewBookStoreApplication.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Country { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}