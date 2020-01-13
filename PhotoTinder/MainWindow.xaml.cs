using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace PhotoTinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PhotoList listOfPhotos = new PhotoList();
        private int photoIndex;

        string acceptedPhotosName = "Accepted Photos";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnNextPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            if (photoIndex < listOfPhotos.Length() - 1)
                photoIndex++;
            else
                photoIndex = 0;

            ImgDisplayedPhoto.Source = listOfPhotos.GetPhoto(photoIndex);
        }

        private void BtnPreviousPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            if (photoIndex > 0)
                photoIndex--;
            else
                photoIndex = listOfPhotos.Length() - 1;

            ImgDisplayedPhoto.Source = listOfPhotos.GetPhoto(photoIndex);
        }

        private void BtnRemovePhoto_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnAcceptPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(acceptedPhotosName);
        }

        private void BtnChooseDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            listOfPhotos.Clear();
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var openFile in openFileDialog.FileNames)
                {
                    var fileUri = new Uri(openFile);
                    listOfPhotos.AddPhoto(new BitmapImage(fileUri));
                    
                }
            }

            ImgDisplayedPhoto.Source = listOfPhotos.GetPhoto();
        }
    }
}
