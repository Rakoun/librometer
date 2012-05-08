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
    public partial class Author : BaseModel
    {
        //private int _idAuthor;
        //[Column(IsPrimaryKey = true, DbType = "UNIQUEIDENTIFIER NOT NULL IDENTITY")]
        //public int IdAuthor
        //{
        //    get
        //    {
        //        return _idAuthor;
        //    }
        //    set
        //    {
        //        if (_idAuthor == value) return;
        //        RaisePropertyChanging<int>(() => IdAuthor);
        //        _idAuthor = value;
        //        RaisePropertyChanged<int>(() => IdAuthor);
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
        #region Relation (1,n)

        private EntitySet<Book> _books = new EntitySet<Book>();
        [Association(OtherKey = "IdAuthor")]
        public EntitySet<Book> Books
        {
            get { return this._books; }
            set { this._books.Assign(value); }
        }

        #endregion

        private string _firstName;
        [Column(CanBeNull=false)]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (_firstName == value) return;
                RaisePropertyChanging<string>(() => FirstName);
                _firstName = value;
                RaisePropertyChanged<string>(() => FirstName);
            }
        }

        private string _lastName;
        [Column(CanBeNull=false)]
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (_lastName == value) return;
                RaisePropertyChanging<string>(() => LastName);
                _lastName = value;
                RaisePropertyChanged<string>(() => LastName);
            }
        }

        private string _displayName;
        [Column(CanBeNull=false)]
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if (_displayName == value) return;
                RaisePropertyChanging<string>(() => DisplayName);
                _displayName = value;
                RaisePropertyChanged<string>(() => DisplayName);
            }
        }
    }
}
