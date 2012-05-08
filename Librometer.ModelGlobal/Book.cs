using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.ModelGlobal
{
    public class Book
    {
        public int IdBook { get; set; }
        public int IdCategory { get; set; }
        public int IdAuthor { get; set; }
        public string Title { get; set; }
        public string Editor { get; set; }
        public int Rate { get; set; }
        public string Cover { get; set; }
        public string ISBN { get; set; }
    }
}
