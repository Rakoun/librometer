using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.ComponentModel.DataAnnotations;non supporté dans WP7
using Librometer.DataAnnotations;

namespace Librometer.Model
{
    public partial class NoteModel : BaseModel
    {
        private int _idNote;
        public int IdNote
        {
            get
            {
                return _idNote;
            }
            set
            {
                if (_idNote == value) return;
                RaisePropertyChanging<int>(() => IdNote);
                _idNote = value;
                RaisePropertyChanged<int>(() => IdNote);
            }
        }

        private int _idBookmark;
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

        private int _notPage;
        [Required]
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
        [Required]
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
        [Required]
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
