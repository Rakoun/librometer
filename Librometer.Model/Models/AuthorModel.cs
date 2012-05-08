using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.ComponentModel.DataAnnotations;non supporté dans WP7
using Librometer.DataAnnotations;

namespace Librometer.Model
{
    public partial class AuthorModel : BaseModel
    {
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

        private string _firstName;
        [Required]
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
        [Required]
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
