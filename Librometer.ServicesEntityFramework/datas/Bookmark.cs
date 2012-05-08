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
    public partial class Bookmark : BaseModel
    {
        //private int _idBook;
        //[Column(IsPrimaryKey = true, DbType = "UNIQUEIDENTIFIER NOT NULL IDENTITY")]
        //public int IdBook
        //{
        //    get
        //    {
        //        return _idBook;
        //    }
        //    set
        //    {
        //        if (_idBook == value) return;
        //        RaisePropertyChanging<int>(() => IdBook);
        //        _idBook = value;
        //        RaisePropertyChanged<int>(() => IdBook);
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

        #region Relation (1,1)

        private EntitySet<Book> _books = new EntitySet<Book>();
        [Association(OtherKey="Id")]
        public EntitySet<Book> Books
        {
            get {return this._books;}
            set {this._books.Assign(value);}
        }

        #endregion

        #region Relation (1,n)

        private EntitySet<Note> _notes = new EntitySet<Note>();
        [Association(OtherKey = "Id")]
        public EntitySet<Note> Notes
        {
            get { return this._notes; }
            set { this._notes.Assign(value); }
        }

        #endregion


        private int _readerPage;
        [Column(CanBeNull=false)]
        public int ReaderPage
        {
            get
            {
                return _readerPage;
            }
            set
            {
                if (_readerPage == value) return;
                RaisePropertyChanging<int>(() => ReaderPage);
                _readerPage = value;
                RaisePropertyChanged<int>(() => ReaderPage);
            }
        }

        private string _thumbImage;
        [Column(CanBeNull=true)]
        public string ThumbImage
        {
            get
            {
                return _thumbImage;
            }
            set
            {
                if (_thumbImage == value) return;
                RaisePropertyChanging<string>(() => ThumbImage);
                _thumbImage = value;
                RaisePropertyChanged<string>(() => ThumbImage);
            }
        }

        private string _name;
        [Column(CanBeNull=false)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value) return;
                RaisePropertyChanging<string>(() => Name);
                _name = value;
                RaisePropertyChanged<string>(() => Name);
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
