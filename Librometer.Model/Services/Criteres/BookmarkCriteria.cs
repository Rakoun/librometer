using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.Model.Services
{
    public class BookmarkCriteria : BaseCriteria<BookmarkCriteria>
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
