using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
//using Microsoft.Win32;

namespace PhotoTinder
{
    public class PhotoManager
    {
        private PhotoList _listOfPhotos;
        private string _acceptedPhotosName = "Accepted Photos";     //folder name for accepted photos
        private string _removedPhotosName = "Removed Photos";       //folder name for removed photos

        public PhotoManager()
        {
            _listOfPhotos = new PhotoList();
        }

        public void ChoosePhotos()
        {
            _listOfPhotos.Clear();

            using (var openFileDialog = new OpenFileDialog {Multiselect = true})
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK) //load selected photos to _listOfPhotos
                {
                    foreach (var openFile in openFileDialog.FileNames)
                    {
                        var fileUri = new Uri(openFile);
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
            return _listOfPhotos.GetNextPhoto();
        }

        public BitmapImage GetPreviousPhoto()
        {
            return _listOfPhotos.GetPreviousPhoto();
        }

        public BitmapImage GetActivePhoto()
        {
            if(_listOfPhotos.GetActivePhoto() == null)
                return new BitmapImage(new Uri(@"https://upload.wikimedia.org/wikipedia/commons/3/30/Googlelogo.png"));
            else
                return _listOfPhotos.GetActivePhoto();
        }

        public void DeletePhoto()
        {
            if (!Directory.Exists(_removedPhotosName))
                Directory.CreateDirectory(_removedPhotosName);

            File.Move(_listOfPhotos.GetActivePhotoUri().AbsolutePath, _removedPhotosName + @"\" + _listOfPhotos.GetActivePhotoFileName());
            
            _listOfPhotos.RemoveActivePhoto();
        }

        /// <summary>
        /// Moves active photo to folder with accepted photos. Creates that folder if it doesn't exist. Don't forget to update displayed photo (using getActivePhoto method).
        /// </summary>
        public void AcceptPhoto()       
        {
            if (!Directory.Exists(_acceptedPhotosName))
                Directory.CreateDirectory(_acceptedPhotosName);

            File.Move(_listOfPhotos.GetActivePhotoUri().AbsolutePath, _acceptedPhotosName + @"\" + _listOfPhotos.GetActivePhotoFileName());

            _listOfPhotos.RemoveActivePhoto();
        }
    }
}