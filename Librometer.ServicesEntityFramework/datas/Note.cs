using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Librometer.Model;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Librometer.ServicesSQLCE.datas
{
    [Table]
    public partial class Note : BaseModel
    {
        //private int _idNote;
        //[Column(IsPrimaryKey = true, DbType = "UNIQUEIDENTIFIER NOT NULL IDENTITY")]
        //public int IdNote
        //{
        //    get
        //    {
        //        return _idNote;
        //    }
        //    set
        //    {
        //        if (_idNote == value) return;
        //        RaisePropertyChanging<int>(() => IdNote);
        //        _idNote = value;
        //        RaisePropertyChanged<int>(() => IdNote);
        //    }
        //}

        [Column(IsPrimaryKey = true, IsDbGenerated=true, DbType = "INT NOT NULL IDENTITY", CanBeNull=false)]
        public override int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value) return;
                RaisePropertyChanging<int>(() => Id);
                _id = value;
                RaisePropertyChanged<int>(() => Id);
            }
        }

        private int _idBookmark;
        [Column(CanBeNull=false)]
        public int IdBookmark
        {
            get
            {
                return _idBookmark;
            }
            set
            {
                if (_idBookmark == value) return;
                RaisePropertyChanging<int>(() => IdBookmark);
                _idBookmark = value;
                RaisePropertyChanged<int>(() => IdBookmark);
            }
        }

        #region Relation (1,n)

        private EntityRef<Bookmark> _bookmarkRef;
        [Association(ThisKey = "IdBookmark", OtherKey="Id", IsForeignKey=true, Storage="_bookmarkRef")]
        public Bookmark Bookmark
        {
            get { return this._bookmarkRef.Entity; }
            set { this._bookmarkRef.Entity = value; }
        }

        #endregion

        private int _notPage;
        [Column(CanBeNull=false)]
        public int NotePage
        {
            get
            {
                return _notPage;
            }
            set
            {
                if (_notPage == value) return;
                RaisePropertyChanging<int>(() => NotePage);
                _notPage = value;
                RaisePropertyChanged<int>(() => NotePage);
            }
        }

        private string _content;
        [Column(CanBeNull=true)]
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content == value) return;
                RaisePropertyChanging<string>(() => Content);
                _content = value;
                RaisePropertyChanged<string>(() => Content);
            }
        }

        private string _creationDate;
        [Column(CanBeNull = false)]
        public string CreationDate
        {
            get
            {
                return _creationDate;
            }
            set
            {
                if (_creationDate == value) return;
                RaisePropertyChanging<string>(() => CreationDate);
                _creationDate = value;
                RaisePropertyChanged<string>(() => CreationDate);
            }
        }
    }
}
