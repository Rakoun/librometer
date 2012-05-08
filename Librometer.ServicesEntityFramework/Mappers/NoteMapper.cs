using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Librometer.Model;
//using Librometer.ModelGlobal;
using Librometer.ServicesSQLCE.datas;

namespace Librometer.ServicesSQLCE.Mappers
{
    public static class NoteMapper
    {
        public static NoteModel ToNoteModel(this Note global)
        {
            NoteModel note = new NoteModel
            {
                Id = global.Id,
                Content = global.Content,
                IdBookmark = global.IdBookmark,
                NotePage = global.NotePage
            };

            return note;
        }

        public static Note ToNote(this NoteModel client)
        {
            Note global = new Note
            {
                Id = client.Id,
                Content = client.Content,
                IdBookmark = client.IdBookmark,
                NotePage = client.NotePage
            };

            return global;
        }
    }
}
