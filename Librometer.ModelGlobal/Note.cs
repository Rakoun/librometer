using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.ModelGlobal
{
    public class Note
    {
        public int IdNote { get; set; }
        public int IdBookmark { get; set; }
        public int NotePage { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
