using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.Model.Services
{
    public class CategoryCriteria : BaseCriteria<CategoryCriteria>
    {
        public string Name { get; set; }
    }
}
