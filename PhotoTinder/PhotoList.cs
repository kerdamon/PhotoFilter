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
        private string ActivePhotoFileName => _listOfPhotos.ElementAt(ActivePhotoIndex).Key;
        private BitmapImage ActivePhotoBitmapImage => _listOfPhotos.ElementAt(ActivePhotoIndex).Value;



        public PhotoList()
        {
            _listOfPhotos = new Dictionary<string, BitmapImage>();
        }

        public BitmapImage GetActivePhoto()
        {
            return GetPhoto(null, 0);
        }

        public BitmapImage GetNextPhoto()
        {
            return GetPhoto(IncrementPhotoIndex, -5);
        }

        public BitmapImage GetPreviousPhoto()
        {
            return GetPhoto(DecrementPhotoIndex, 5);
        }

        /// <summary>
        /// Loads previous, current or next photo depending on first argument, which is function incrementing or decrementing photo index. Second argument is index (relative to active photo index) indicating photo that will be unloaded (for memory optimization purposes). If second argument is 0, none photo will be unloaded.
        /// </summary>
        private BitmapImage GetPhoto(Action changeIndex, int unloadIndex)
        {
            if (IsEmpty()) return null;

            changeIndex?.Invoke();

            if (unloadIndex != 0)
            {
                var offsetPhotoIndex = ActivePhotoIndex + unloadIndex;
                if (offsetPhotoIndex < _listOfPhotos.Count && offsetPhotoIndex >= 0 && _listOfPhotos.ElementAt(offsetPhotoIndex).Value != null)
                    UnLoadImage(_listOfPhotos.ElementAt(offsetPhotoIndex).Key);
            }

            if (ActivePhotoBitmapImage == null)
                LoadImage(ActivePhotoFileName);

            return ActivePhotoBitmapImage;
        }

        private void IncrementPhotoIndex()
        {
            if (ActivePhotoIndex >= (_listOfPhotos.Count - 1))
                ActivePhotoIndex = 0;
            else
                ActivePhotoIndex++;
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
            return IsEmpty() ? null : ActivePhotoFileName;
        }

        public string GetActivePhotoFileName()
        {
            return ActivePhotoFileName.Split('\\').Last();
        }

        public void RemoveActivePhoto()
        {
            _listOfPhotos.Remove(ActivePhotoFileName);
            if (ActivePhotoIndex >= (_listOfPhotos.Count - 1))
                ActivePhotoIndex = 0;
        }

        public void AddPhoto(string fileName)
        {
            if (HasImageExtension(fileName))
                _listOfPhotos.Add(fileName, null);
        }

        private static bool HasImageExtension(string fileName)
        {
            return new[] {".jpg", ".jpeg", ".png"}.Contains(Path.GetExtension(fileName));
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