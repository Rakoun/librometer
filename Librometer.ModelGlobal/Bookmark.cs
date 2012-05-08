using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.ModelGlobal
{
    public class Bookmark
    {
        public int IdBook { get; set; }
        public int ReaderPage { get; set; }
        public string ThumbImage { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
