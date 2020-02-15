using System;
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
            var photo = _listOfPhotos.ElementAt(ActivePhotoIndex);
            return photo.Value;
        }

        public BitmapImage GetNextPhoto()
        {
            if (IsEmpty()) return null;
            IncrementPhotoIndex();
            var photo = _listOfPhotos.ElementAt(ActivePhotoIndex);
            return photo.Value;
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
            DecrementPhotoIndex();
            var photo = _listOfPhotos.ElementAt(ActivePhotoIndex);
            return photo.Value;
        }

        private void DecrementPhotoIndex()
        {
            if (ActivePhotoIndex <= 0)
                ActivePhotoIndex = _listOfPhotos.Count - 1;
            else
                ActivePhotoIndex--;
        }

        public Uri GetActivePhotoUri()
        {
            if (_listOfPhotos.Count == 0) return null;
            var photo = _listOfPhotos.ElementAt(ActivePhotoIndex);
            return new Uri(photo.Key);
        }

        public string GetActivePhotoFileName()
        {
            var temp = _listOfPhotos.ElementAt(ActivePhotoIndex).Key.Split('\\');
            return temp[temp.Length - 1];
        }

        public void RemoveActivePhoto()
        {
            _listOfPhotos.Remove(_listOfPhotos.ElementAt(ActivePhotoIndex).Key);
            IncrementPhotoIndex();
        }

        public void AddPhoto(string fileName)
        {
            var image = new BitmapImage();

            using (var stream = File.OpenRead(fileName))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }

            _listOfPhotos.Add(fileName, image);
        }

        public int Length()
        {
            return _listOfPhotos.Count;
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
    }
}