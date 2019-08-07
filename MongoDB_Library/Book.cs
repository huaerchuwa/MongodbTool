using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
namespace MongoDB_Library
{
    public class Book
    {
        private string p;

        public Book(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }

        public Book()
        {
            // TODO: Complete member initialization
        }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string BorrowedBookName  { get; set; }
        public string BorrowDate { get; set; }
        public string ReturnDate { get; set; }
    }
}
