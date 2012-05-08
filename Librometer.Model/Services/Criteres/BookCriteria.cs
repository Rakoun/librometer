using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.Model.Services
{
    public class BookCriteria : BaseCriteria<BookCriteria>
    {
        public string Title { get; set; }
    }
}
