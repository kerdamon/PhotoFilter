using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace PhotoTinder
{
    public class PhotoList
    {
        private readonly Dictionary<string, BitmapImage> _listOfPhotos;
        public int ActivePhotoIndex { get; private set; }

        public PhotoList()
        {
            _listOfPhotos = new Dictionary<string, BitmapImage>();
        }

        public BitmapImage GetActivePhoto()
        {
            if (IsEmpty()) return null;
            if (_listOfPhotos.ElementAt(ActivePhotoIndex).Value == null)
                LoadImage(_listOfPhotos.ElementAt(ActivePhotoIndex).Key);
            return _listOfPhotos.ElementAt(ActivePhotoIndex).Value;
        }

        public BitmapImage GetNextPhoto()
        {
            if (IsEmpty()) return null;
            
            if (_listOfPhotos.ElementAt(ActivePhotoIndex).Value != null)
                UnLoadImage(_listOfPhotos.ElementAt(ActivePhotoIndex).Key);
            
            IncrementPhotoIndex();
            
            if(_listOfPhotos.ElementAt(ActivePhotoIndex).Value == null)
                LoadImage(_listOfPhotos.ElementAt(ActivePhotoIndex).Key);
            
            return _listOfPhotos.ElementAt(ActivePhotoIndex).Value;
        }

        private void IncrementPhotoIndex()
        {
            if (ActivePhotoIndex >= (_listOfPhotos.Count - 1))
                ActivePhotoIndex = 0;
            else
                ActivePhotoIndex++;
        }

        public BitmapImage GetPreviousPhoto()
        {
            if (IsEmpty()) return null;

            if (_listOfPhotos.ElementAt(ActivePhotoIndex).Value != null)
                UnLoadImage(_listOfPhotos.ElementAt(ActivePhotoIndex).Key);

            DecrementPhotoIndex();

            if (_listOfPhotos.ElementAt(ActivePhotoIndex).Value == null)
                LoadImage(_listOfPhotos.ElementAt(ActivePhotoIndex).Key);

            return _listOfPhotos.ElementAt(ActivePhotoIndex).Value;
        }

        private void DecrementPhotoIndex()
        {
            if (ActivePhotoIndex <= 0)
                ActivePhotoIndex = _listOfPhotos.Count - 1;
            else
                ActivePhotoIndex--;
        }

        public string GetActivePhotoPath()
        {
            return IsEmpty() ? null : _listOfPhotos.ElementAt(ActivePhotoIndex).Key;
        }

        public string GetActivePhotoFileName()
        {
            var temp = _listOfPhotos.ElementAt(ActivePhotoIndex).Key.Split('\\');
            return temp[temp.Length - 1];
        }

        public void RemoveActivePhoto()
        {
            _listOfPhotos.Remove(_listOfPhotos.ElementAt(ActivePhotoIndex).Key);
            if (ActivePhotoIndex >= (_listOfPhotos.Count - 1))
                ActivePhotoIndex = 0;
        }

        public void AddPhoto(string fileName)
        {
            if (HasImageExtension(fileName))   //to change to be more general
                _listOfPhotos.Add(fileName, null);
        }

        private static bool HasImageExtension(string fileName)
        {
            return (Path.GetExtension(fileName) == ".jpg" || Path.GetExtension(fileName) == ".jpeg" ||
                    Path.GetExtension(fileName) == ".png");
        }

        public bool IsEmpty()
        {
            return _listOfPhotos.Count <= 0;
        }

        public void Clear()
        {
            _listOfPhotos.Clear();
            ActivePhotoIndex = 0;
        }

        private void LoadImage(string fileName)
        {
            var image = new BitmapImage();

            using (var stream = File.OpenRead(fileName))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }

            _listOfPhotos[fileName] = image;
        }

        private void UnLoadImage(string fileName)
        {
            _listOfPhotos[fileName] = null;
        }

    }
}