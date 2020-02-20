using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace PhotoTinder
{
    public class PhotoManager
    {
        private readonly PhotoList _listOfPhotos;
        private const string AcceptedPhotosName = "Accepted Photos"; //folder name for accepted photos
        private const string RemovedPhotosName = "Removed Photos"; //folder name for removed photos
        private string _acceptedPhotosPath = "";
        private string _removedPhotosPath = "";

        public PhotoManager()
        {
            _listOfPhotos = new PhotoList();
        }

        public void ChoosePhotoFiles()
        {
            _listOfPhotos.Clear();

            using (var openFileDialog = new OpenFileDialog {Multiselect = true, Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK) //load selected photos to _listOfPhotos
                {
                    foreach (var openFile in openFileDialog.FileNames)
                    {
                        _listOfPhotos.AddPhoto(openFile);
                    }
                }

                _acceptedPhotosPath =
                    Regex.Replace(openFileDialog.FileName, @"[^\\]*$", @"") +
                    AcceptedPhotosName; //set the path for accepted photos folder

                _removedPhotosPath=
                    Regex.Replace(openFileDialog.FileName, @"[^\\]*$", @"") +
                    RemovedPhotosName; //set the path for removed photos folder
            }
        }

        public void ChoosePhotoFolder()
        {
            _listOfPhotos.Clear();

            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    foreach (var openFile in Directory.GetFiles(fbd.SelectedPath))
                    {
                        _listOfPhotos.AddPhoto(openFile);
                    }
                }

                _acceptedPhotosPath = fbd.SelectedPath + @"\" + AcceptedPhotosName;
                _removedPhotosPath = fbd.SelectedPath + @"\" + RemovedPhotosName;
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
            if (!Directory.Exists(_removedPhotosPath))
                Directory.CreateDirectory(_removedPhotosPath);

            File.Move(_listOfPhotos.GetActivePhotoPath(), _removedPhotosPath + @"\" + _listOfPhotos.GetActivePhotoFileName());

            _listOfPhotos.RemoveActivePhoto();
        }

        /// <summary>
        /// Moves active photo to folder with accepted photos. Creates that folder if it doesn't exist. Don't forget to update displayed photo (using getActivePhoto method).                                                                                                                       
        /// </summary>
        public void AcceptPhoto()       
        {
            if (_listOfPhotos.IsEmpty()) return;
            if (!Directory.Exists(_acceptedPhotosPath))
                Directory.CreateDirectory(_acceptedPhotosPath);

            File.Move(_listOfPhotos.GetActivePhotoPath(), _acceptedPhotosPath + @"\" + _listOfPhotos.GetActivePhotoFileName());

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

        public void OpenActivePhotoInImageViewer()
        {
            if(!_listOfPhotos.IsEmpty())
                System.Diagnostics.Process.Start(_listOfPhotos.GetActivePhotoPath());
        }
    }


}