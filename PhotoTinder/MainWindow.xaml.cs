using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private PhotoManager photoManager = new PhotoManager();
        string acceptedPhotosName = "Accepted Photos";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnNextPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            ImgDisplayedPhoto.Source = photoManager.GetNextPhoto();
        }

        private void BtnPreviousPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            ImgDisplayedPhoto.Source = photoManager.GetPreviousPhoto();
        }

        private void BtnDeletePhoto_OnClick(object sender, RoutedEventArgs e)
        {
            photoManager.DeletePhoto();
            ImgDisplayedPhoto.Source = photoManager.GetActivePhoto();

            //if ((ImgDisplayedPhoto.Source = listOfPhotos.GetNextPhoto()) == null)
            //{
            //    ImgDisplayedPhoto.Source = new BitmapImage(new Uri("https://upload.wikimedia.org/wikipedia/commons/3/30/Googlelogo.png"));
            //}
            //File.Delete(listOfPhotos.GetActivePhotoUri().AbsolutePath);
        }

        private void BtnAcceptPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            photoManager.AcceptPhoto();
            ImgDisplayedPhoto.Source = photoManager.GetActivePhoto();
        }

        private void BtnChooseFiles_OnClick(object sender, RoutedEventArgs e)
        {
            photoManager.ChoosePhotos();
            ImgDisplayedPhoto.Source = photoManager.GetActivePhoto();       //shows first photo after files were chosen
        }
    }
}