using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.Model.Services
{
    public class NoteCriteria : BaseCriteria<NoteCriteria>
    {
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
