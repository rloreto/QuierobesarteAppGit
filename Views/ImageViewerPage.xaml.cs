using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Practices.ServiceLocation;
using QuierobesarteApp.ViewModel;
using QuierobesarteApp.Model;

namespace QuierobesarteApp.Views
{
    public partial class ImageViewerPage : PhoneApplicationPage
    {
        public ImageViewerPage()
        {
            InitializeComponent();
        }
        string _weddingId;


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.TryGetValue("weddingId", out _weddingId))
            {
                var client = new HttpClient();


                HttpResponseMessage getresponse = await client.GetAsync("http://" + App.baseUrl + "/api/images/" + _weddingId + "?page=0&numItems=30");
                string json = await getresponse.Content.ReadAsStringAsync();
                var images = JsonConvert.DeserializeObject<List<ImageDto>>(json);

                foreach (var item in images)
                {
                    item.originalPath = "http://" + App.baseUrl + item.originalPath;
                    item.thumbnailPath = "http://" + App.baseUrl + item.thumbnailPath;
                }

                var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
                mainViewModel.Images = new System.Collections.ObjectModel.ObservableCollection<ImageDto>(images);

            }
        }


        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/Views/ImageDetailPage.xaml", UriKind.Relative));
            });
        }
    }
}