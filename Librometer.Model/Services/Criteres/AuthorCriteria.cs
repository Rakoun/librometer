using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.Model.Services
{
    public class AuthorCriteria : BaseCriteria<AuthorCriteria>
    {
        public string LastName { get; set; }
    }
}
