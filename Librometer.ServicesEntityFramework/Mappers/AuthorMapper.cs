using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Librometer.Model;
//using Librometer.ModelGlobal;
using Librometer.ServicesSQLCE.datas;

namespace Librometer.ServicesSQLCE.Mappers
{
    public static class AuthorMapper
    {
        public static AuthorModel ToAuthorModel(this Author global)
        {
            AuthorModel author = new AuthorModel
            {
                DisplayName = global.DisplayName,
                FirstName = global.FirstName,
                Id = global.Id,
                LastName = global.LastName
            };

            return author;
        }

        public static Author ToAuthor(this AuthorModel client)
        {
            Author global = new Author
            {
                DisplayName = client.DisplayName,
                FirstName = client.FirstName,
                Id = client.Id,
                LastName = client.LastName
            };

            return global;
        }
    }
}
