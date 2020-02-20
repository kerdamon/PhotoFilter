using System.Windows;

namespace PhotoTinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PhotoManager _photoManager = new PhotoManager();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnNextPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            ImgDisplayedPhoto.Source = _photoManager.GetNextPhoto();
        }

        private void BtnPreviousPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            ImgDisplayedPhoto.Source = _photoManager.GetPreviousPhoto();
        }

        private void BtnDeletePhoto_OnClick(object sender, RoutedEventArgs e)
        {
            _photoManager.DeletePhoto();
            ImgDisplayedPhoto.Source = _photoManager.GetActivePhoto();
        }

        private void BtnAcceptPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            _photoManager.AcceptPhoto();
            ImgDisplayedPhoto.Source = _photoManager.GetActivePhoto();
        }

        private void BtnChooseFiles_OnClick(object sender, RoutedEventArgs e)
        {
            _photoManager.ChoosePhotoFiles();
            ImgDisplayedPhoto.Source = _photoManager.GetActivePhoto();       //shows first photo after files were chosen
        }

        private void BtnOpenInImageViewer_OnClick(object sender, RoutedEventArgs e)
        {
            _photoManager.OpenActivePhotoInImageViewer();
        }

        private void BtnChoosePhotoFolder_OnClick(object sender, RoutedEventArgs e)
        {
            _photoManager.ChoosePhotoFolder();
            ImgDisplayedPhoto.Source = _photoManager.GetActivePhoto();
        }
    }
}