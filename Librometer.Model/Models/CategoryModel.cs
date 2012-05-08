using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.ComponentModel.DataAnnotations;non supporté dans WP7
using Librometer.DataAnnotations;


namespace Librometer.Model
{
    public partial class CategoryModel : BaseModel
    {
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

        private string _name;
        [Required]
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
