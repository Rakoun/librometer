using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;


namespace Librometer.ServicesSQLCE.datas
{
    public class LibroContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/LibroDb.sdf";

        public LibroContext(string connectionString):base(connectionString)
        {
            this.Authors = this.GetTable<Author>();
            this.Books = this.GetTable<Book>();
            this.Bookmarks = this.GetTable<Bookmark>();
            this.Categories = this.GetTable<Category>();
            this.Notes = this.GetTable<Note>();
        }

        public Table<Author> Authors {get;set;}
        public Table<Book> Books { get; set; }
        public Table<Bookmark> Bookmarks { get; set; }
        public Table<Category> Categories { get; set; }
        public Table<Note> Notes { get; set; }
        
    }
}
