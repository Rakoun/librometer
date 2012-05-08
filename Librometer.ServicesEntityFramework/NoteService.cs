using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Librometer.Model;
using Librometer.Model.Services;
using Librometer.ServicesSQLCE.Mappers;
using Librometer.ServicesSQLCE.datas;

namespace Librometer.ServicesSQLCE
{
    public partial class NoteService
        : BaseService<NoteModel, Note, NoteCriteria>
        , INoteService
    {
        #region Méthodes spécifiques à IBookService
        //TODO: si nécessaire
        #endregion


        #region Méthodes surchargée de ServiceBase
        protected override Note MapToEntity(NoteModel model)
        {
            var global = model.ToNote();
            return global;
        }

        protected override NoteModel MapToModel(Note entity)
        {
            var Note = entity.ToNoteModel();

            return Note;
        }

        protected override IQueryable<Note> GetConcrete(NoteCriteria criteria)
        {
            Thread.Sleep(3000);

            IQueryable<Note> notes =
                notes = from Note note in Context.Notes
                        select note;

            if (!string.IsNullOrEmpty(criteria.Content))
                notes = notes.Where(n => n.Content.Contains(criteria.Content));

            return notes;
        }

        protected override void DeepCopy(Note oldEntity, Note newEntity)
        {
            oldEntity.Id = newEntity.Id;
            oldEntity.Content = newEntity.Content;
            oldEntity.IdBookmark = newEntity.IdBookmark;
            oldEntity.NotePage = newEntity.NotePage;
        }

        #endregion // Méthodes surchargée de ServiceBase
    }
}

