using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace PhotoTinder
{
    public class PhotoManager
    {
        private readonly PhotoList _listOfPhotos;
        private string _acceptedPhotosName = "Accepted Photos";     //folder name for accepted photos
        private string _removedPhotosName = "Removed Photos";       //folder name for removed photos

        public PhotoManager()
        {
            _listOfPhotos = new PhotoList();
        }

        public void ChoosePhotos()
        {
            _listOfPhotos.Clear();

            using (var openFileDialog = new OpenFileDialog {Multiselect = true, Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK) //load selected photos to _listOfPhotos
                {
                    foreach (var openFile in openFileDialog.FileNames)
                    {
                        _listOfPhotos.AddPhoto(openFile);
                    }
                }

                _acceptedPhotosName =
                    Regex.Replace(openFileDialog.FileName, @"[^\\]*$", @"") +
                    _acceptedPhotosName; //set the path for accepted photos folder

                _removedPhotosName=
                    Regex.Replace(openFileDialog.FileName, @"[^\\]*$", @"") +
                    _removedPhotosName; //set the path for removed photos folder
            }
        }

        public BitmapImage GetNextPhoto()
        {
            return _listOfPhotos.IsEmpty() ? GetClosingImage() : _listOfPhotos.GetNextPhoto();
        }

        public BitmapImage GetPreviousPhoto()
        {
            return _listOfPhotos.IsEmpty() ? GetClosingImage() : _listOfPhotos.GetPreviousPhoto();
        }

        public BitmapImage GetActivePhoto()
        {
            return _listOfPhotos.GetActivePhoto() ?? GetClosingImage();
        }

        public void DeletePhoto()
        {
            if (_listOfPhotos.IsEmpty()) return;
            if (!Directory.Exists(_removedPhotosName))
                Directory.CreateDirectory(_removedPhotosName);

            File.Move(_listOfPhotos.GetActivePhotoPath(), _removedPhotosName + @"\" + _listOfPhotos.GetActivePhotoFileName());

            _listOfPhotos.RemoveActivePhoto();
        }

        /// <summary>
        /// Moves active photo to folder with accepted photos. Creates that folder if it doesn't exist. Don't forget to update displayed photo (using getActivePhoto method).                                                                                                                       
        /// </summary>
        public void AcceptPhoto()       
        {
            if (_listOfPhotos.IsEmpty()) return;
            if (!Directory.Exists(_acceptedPhotosName))
                Directory.CreateDirectory(_acceptedPhotosName);

            File.Move(_listOfPhotos.GetActivePhotoPath(), _acceptedPhotosName + @"\" + _listOfPhotos.GetActivePhotoFileName());

            _listOfPhotos.RemoveActivePhoto();
        }

        private static BitmapImage GetClosingImage()
        {
            return BitmapToBitmapImage(Properties.Resources.ClosingImage);
        }
        /// <summary>
        /// Function borrowed from StackOverflow to convert Bitmap to BitmapImage.
        /// </summary>
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

    }


}