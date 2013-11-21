using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.Net.Http;
using QuierobesarteApp.Model;
using Windows.Storage;
using Windows.Storage.Streams;

namespace QuierobesarteApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
         
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.CurrentNavigationService == null)
            {
                App.CurrentNavigationService = base.NavigationService;
            }
        }

       

        private async void Enviar_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = System.Windows.Visibility.Visible;
            btnEnviar.IsEnabled = false;

            var weddingId = this.weddingPublicId.Text;

            if (weddingPublicId.Text != string.Empty)
            {
                var client = new HttpClient();
                await client.GetStringAsync("http://" + App.baseUrl + "/api/Wedding/" + weddingId)
                   .ContinueWith(t =>
                   {

                       var wedding = JsonConvert.DeserializeObject<WeddingDto>(t.Result);
                       if (wedding != null)
                       {
                           Dispatcher.BeginInvoke(() =>
                           {
                               AppModel appModel = new AppModel() { CurrentWeddingId = weddingId };
                               Utils.SerializeAppData(appModel);
                               NavigationService.Navigate(new Uri("/Views/UploaderPage.xaml?weddingId=" + weddingId, UriKind.Relative));

                           });

                       }
                       else
                       {
                           ShowErrorMessage(weddingId);

                       }

                       Dispatcher.BeginInvoke(() =>
                       {
                           progressBar.Visibility = System.Windows.Visibility.Collapsed;
                           btnEnviar.IsEnabled = true;
                       });

                   });

            }

        }
        void ShowErrorMessage(string weddingPublicId)
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("El número '" + weddingPublicId + "' no corresponde a ninguna boda activa. ¡Verifica el número e intentalo de nuevo!.");
            });
        }


    }
}