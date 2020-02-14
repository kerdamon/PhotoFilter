using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace PhotoTinder
{
    public class PhotoList
    {
        private Dictionary<Uri, BitmapImage> _listOfPhotos;
        public int ActivePhotoIndex { get; private set; }

        public PhotoList()
        {
            _listOfPhotos = new Dictionary<Uri, BitmapImage>();
        }

        public BitmapImage GetActivePhoto()
        {
            if (_listOfPhotos.Count == 0) return null;
            var photo = _listOfPhotos.ElementAt(ActivePhotoIndex);
            return photo.Value;
        }

        public BitmapImage GetNextPhoto()
        {
            if (_listOfPhotos.Count == 0) return null;
            if (ActivePhotoIndex >= (_listOfPhotos.Count - 1))
            {
                ActivePhotoIndex = 0;
            }
            else
            {
                ActivePhotoIndex++;
            }

            var photo = _listOfPhotos.ElementAt(ActivePhotoIndex);
            return photo.Value;
        }

        public BitmapImage GetPreviousPhoto()
        {
            if (_listOfPhotos.Count == 0) return null;
            if (ActivePhotoIndex <= 0)
            {
                ActivePhotoIndex = _listOfPhotos.Count - 1;
            }
            else
            {
                ActivePhotoIndex--;
            }

            var photo = _listOfPhotos.ElementAt(ActivePhotoIndex);
            return photo.Value;
        }

        public Uri GetActivePhotoUri()
        {
            if (_listOfPhotos.Count == 0) return null;
            var photo = _listOfPhotos.ElementAt(ActivePhotoIndex);
            return photo.Key;
        }

        public string GetActivePhotoFileName()
        {
            var temp = _listOfPhotos.ElementAt(ActivePhotoIndex).Key.OriginalString.Split('\\');
            return temp[temp.Length - 1];
        }

        public void RemoveActivePhoto()
        {
            _listOfPhotos.Remove(_listOfPhotos.ElementAt(ActivePhotoIndex).Key);
        }

        public void AddPhoto(Uri fileUri)
        {
            _listOfPhotos.Add(fileUri, new BitmapImage(fileUri));
        }

        public int Length()
        {
            return _listOfPhotos.Count;
        }

        public void Clear()
        {
            _listOfPhotos.Clear();
            ActivePhotoIndex = 0;
        }
    }
}