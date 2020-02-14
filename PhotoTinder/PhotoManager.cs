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
        private PhotoList _listOfPhotos = new PhotoList();
        private string _acceptedPhotosName = "Accepted Photos";      //folder name for accepted photos
        private string _removedPhotosName = "Removed Photos";
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
                        _listOfPhotos.AddPhoto(fileUri);
                    }
                }

                Trace.WriteLine("acceptedPhotosName : " + _acceptedPhotosName); //for debugging purposes
                Trace.WriteLine("openFileDialog.FileName : " + openFileDialog.FileName); //for debugging purposes
                _acceptedPhotosName =
                    Regex.Replace(openFileDialog.FileName, @"[^\\]*$", @"") +
                    _acceptedPhotosName; //set the path for accepted photos folder
                _removedPhotosName =
                    Regex.Replace(openFileDialog.FileName, @"[^\\]*$", @"") +
                    _removedPhotosName; //set the path for removed photos folder
                Trace.WriteLine("acceptedPhotosName After : " + _acceptedPhotosName); //for debugging purposes
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
            return _listOfPhotos.GetActivePhoto();
        }

        public void DeletePhoto()
        {
            if (!Directory.Exists(_removedPhotosName))
                Directory.CreateDirectory(_removedPhotosName);

            _listOfPhotos.RemoveActivePhoto();
            File.Delete(_listOfPhotos.GetActivePhotoUri().AbsolutePath);


//            File.Copy(_listOfPhotos.GetActivePhotoUri().AbsolutePath, _removedPhotosName + @"\" + _listOfPhotos.GetActivePhotoFileName());
//
//            _listOfPhotos.RemoveActivePhoto();
        }

        /// <summary>
        /// Moves active photo to folder with accepted photos. Creates that folder if it doesn't exist. Don't forget to update displayed photo (using getActivePhoto method).
        /// </summary>
        public void AcceptPhoto()       
        {
            if (!Directory.Exists(_acceptedPhotosName))
                Directory.CreateDirectory(_acceptedPhotosName);

            Trace.WriteLine("Folder path: " + _acceptedPhotosName);  //for debugging purposes
            Trace.WriteLine("Active Photo Uri " + _listOfPhotos.GetActivePhotoUri());

            File.Copy(_listOfPhotos.GetActivePhotoUri().AbsolutePath, _acceptedPhotosName + @"\" + _listOfPhotos.GetActivePhotoFileName());

            _listOfPhotos.RemoveActivePhoto();
        }
    }
}