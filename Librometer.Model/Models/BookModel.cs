using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.ComponentModel.DataAnnotations;non supporté dans WP7
using Librometer.DataAnnotations;

namespace Librometer.Model
{
    public partial class BookModel : BaseModel
    {
        private int _idBook;
        public int IdBook
        {
            get
            {
                return _idBook;
            }
            set
            {
                if (_idBook == value) return;
                RaisePropertyChanging<int>(() => IdBook);
                _idBook = value;
                RaisePropertyChanged<int>(() => IdBook);
            }
        }

        private int _idCategory;
        public int IdCategory
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

        private int _idAuthor;
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
        [Required]
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
