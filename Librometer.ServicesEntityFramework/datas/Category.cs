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
    public partial class Category : BaseModel
    {
        //private int _idCategory;
        //[Column(IsPrimaryKey = true, DbType = "UNIQUEIDENTIFIER NOT NULL IDENTITY")]
        //public int IdCategory
        //{
        //    get
        //    {
        //        return _idCategory;
        //    }
        //    set
        //    {
        //        if (_idCategory == value) return;
        //        RaisePropertyChanging<int>(() => IdCategory);
        //        _idCategory = value;
        //        RaisePropertyChanged<int>(() => IdCategory);
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
        [Association(OtherKey = "IdCategory")]
        public EntitySet<Book> Books
        {
            get { return this._books; }
            set { this._books.Assign(value); }
        }

        #endregion

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
    }
}
