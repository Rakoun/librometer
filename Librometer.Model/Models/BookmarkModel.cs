using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Text;
//using System.ComponentModel.DataAnnotations;non supporté dans WP7
using Librometer.DataAnnotations;
using Rakouncom.WP.IsolatedStorage;


namespace Librometer.Model
{
    public partial class BookmarkModel : BaseModel
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

        private int _readerPage;
        [Required]
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

        public BitmapImage ImageSource
        {
            get
            {
                BitmapImage bi = new BitmapImage();

                using (IsolatedStorageFileStream stream =
                    IsolatedStorageHelper.ReadBinaryFile(_thumbImage))
                {
                    bi.SetSource(stream);
                    stream.Close();
                }
                return bi;
            }
        }
    }
}
