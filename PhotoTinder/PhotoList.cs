using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Windows.Media.Imaging;

namespace PhotoTinder
{
    public class PhotoList
    {
        private List<BitmapImage> listOfPhotos;

        public PhotoList()
        {
            listOfPhotos = new List<BitmapImage>();
        }

        public BitmapImage GetPhoto(int photoIndex = 0)
        {
            return listOfPhotos[photoIndex];
        }

        public void RemovePhoto(int photoIndex = 0)
        {
            listOfPhotos.Remove(listOfPhotos[photoIndex]);
        }

        public void AddPhoto(BitmapImage newImage)
        {
            listOfPhotos.Add(newImage);
        }

        public int Length()
        {
            return listOfPhotos.Count;
        }

        public void Clear()
        {
            listOfPhotos.Clear();
        }
    }
}