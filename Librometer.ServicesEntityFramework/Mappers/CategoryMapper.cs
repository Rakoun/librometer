using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Librometer.Model;
//using Librometer.ModelGlobal;
using Librometer.ServicesSQLCE.datas;

namespace Librometer.ServicesSQLCE.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryModel ToCategoryModel(this Category global)
        {
            CategoryModel category = new CategoryModel
            {
                Id = global.Id,
                Name = global.Name
            };

            return category;
        }

        public static Category ToCategory(this CategoryModel client)
        {
            Category global = new Category
            {
                Id = client.Id,
                Name = client.Name
            };

            return global;
        }
    }
}
