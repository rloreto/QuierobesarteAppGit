using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Net.Http;

namespace QuierobesarteApp.Views
{
    public partial class UploaderPage : PhoneApplicationPage
    {

        PhotoChooserTask _photoChooserTask;
        string _fileName;
        string _weddingId;

        public UploaderPage()
        {
            InitializeComponent();
            _photoChooserTask = new PhotoChooserTask();
            _photoChooserTask.ShowCamera = true;
            _photoChooserTask.Completed +=
                new EventHandler<PhotoResult>(OnPhotoChooserTaskCompleted);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.TryGetValue("weddingId", out _weddingId))
            {
              
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _photoChooserTask.Show();
        }

        private void btnViewImages_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/Views/ImageViewerPage.xaml?weddingId=" + _weddingId, UriKind.Relative));
            });
        }

        private void OnPhotoChooserTaskCompleted(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                _fileName = e.OriginalFileName;


                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(e.ChosenPhoto);
                decimal resizeWidth;
                decimal resizeHeight;
                decimal ratio;

                if (bitmapImage.PixelWidth > bitmapImage.PixelHeight)
                {
                    ratio = (decimal)bitmapImage.PixelWidth / (decimal)bitmapImage.PixelHeight;
                    resizeWidth = 1920;
                    resizeHeight = 1920 / ratio;
                }
                else
                {
                    ratio = (decimal)bitmapImage.PixelHeight / (decimal)bitmapImage.PixelWidth;
                    resizeHeight = 1920;
                    resizeWidth = 1920 / ratio;
                }



                WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapImage);
                MemoryStream ms = new MemoryStream();
                writeableBitmap.SaveJpeg(ms, (int)resizeWidth, (int)resizeHeight, 0, 80);
                UploadFile(ms);

            

            }
        }

        private async void UploadFile(MemoryStream ms)
        {

            btnUploadImage.IsEnabled = false;
            btnViewImages.IsEnabled = false;
            progressBar.Visibility = System.Windows.Visibility.Visible;
            // Make sure there is a picture selected



            if (ms != null)
            {
                var fileUploadUrl = @"http://" + App.baseUrl + "/Uploader/Upload/?guid=" + _weddingId;
                var client = new HttpClient();
                ms.Position = 0;
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StreamContent(ms), "file", _fileName);
                await client.PostAsync(fileUploadUrl, content)
                    .ContinueWith((postTask) =>
                    {
                        EnableButtons();
                        //postTask.Result.EnsureSuccessStatusCode();

                    });
            }



        }

        private void EnableButtons()
        {
            Dispatcher.BeginInvoke(() =>
            {
                btnUploadImage.IsEnabled = true;
                btnViewImages.IsEnabled = true;
                progressBar.Visibility = System.Windows.Visibility.Collapsed;
            });

        }


    }
}