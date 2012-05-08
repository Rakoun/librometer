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
    public partial class Book : BaseModel
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


        private int  _idCategory;
        [Column(CanBeNull=true)]
        public int  IdCategory
        {
            get
            {
                return _idCategory;
            }
            set
            {
                if (_idCategory == value) return;
                RaisePropertyChanging<int>(() => IdCategory);
                _idCategory = value;
                RaisePropertyChanged<int>(() => IdCategory);
            }
        }

        #region Relation (1,n)

        private EntityRef<Category> _categoryRef;
        [Association(ThisKey = "IdCategory", OtherKey = "Id", IsForeignKey = true, Storage = "_categoryRef")]
        public Category Category
        {
            get { return this._categoryRef.Entity; }
            set { this._categoryRef.Entity = value; }
        }

        private EntityRef<Author> _authorRef;
        [Association(ThisKey = "IdAuthor", OtherKey = "Id", IsForeignKey = true, Storage = "_authorRef")]
        public Author Author
        {
            get { return this._authorRef.Entity; }
            set { this._authorRef.Entity = value; }
        }
        #endregion

        #region Relation (1,1)

        private EntitySet<Bookmark> _bookmarks = new EntitySet<Bookmark>();
        [Association(OtherKey = "Id")]
        public EntitySet<Bookmark> Bookmarks
        {
            get { return this._bookmarks; }
            set { this._bookmarks.Assign(value); }
        }

        #endregion

        private int _idAuthor;
        [Column(CanBeNull=false)]
        public int IdAuthor
        {
            get
            {
                return _idAuthor;
            }
            set
            {
                if (_idAuthor == value) return;
                RaisePropertyChanging<int>(() => IdAuthor);
                _idAuthor = value;
                RaisePropertyChanged<int>(() => IdAuthor);
            }
        }

        private string _title;
        [Column(CanBeNull=false)]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title == value) return;
                RaisePropertyChanging<string>(() => Title);
                _title = value;
                RaisePropertyChanged<string>(() => Title);
            }
        }

        private string _editor;
        [Column(CanBeNull=true)]
        public string Editor
        {
            get
            {
                return _editor;
            }
            set
            {
                if (_editor == value) return;
                RaisePropertyChanging<string>(() => Editor);
                _editor = value;
                RaisePropertyChanged<string>(() => Editor);
            }
        }

        private int _rate;
        [Column(CanBeNull=true)]
        public int Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                if (_rate == value) return;
                RaisePropertyChanging<int>(() => Rate);
                _rate = value;
                RaisePropertyChanged<int>(() => Rate);
            }
        }

        private string _cover;
        [Column(CanBeNull=true)]
        public string Cover
        {
            get
            {
                return _cover;
            }
            set
            {
                if (_cover == value) return;
                RaisePropertyChanging<string>(() => Cover);
                _cover = value;
                RaisePropertyChanged<string>(() => Cover);
            }
        }

        private string _isbn;
        [Column(CanBeNull=true)]
        public string ISBN
        {
            get
            {
                return _isbn;
            }
            set
            {
                if (_isbn == value) return;
                RaisePropertyChanging<string>(() => ISBN);
                _isbn = value;
                RaisePropertyChanged<string>(() => ISBN);
            }
        }


    }
}
