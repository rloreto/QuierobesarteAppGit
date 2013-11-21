using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QuierobesarteApp.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace QuierobesarteApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        RelayCommand<string> _onImageTapCommand;

        public RelayCommand<string> OnImageTapCommand
        {
            get { return _onImageTapCommand; }
            private set { _onImageTapCommand = value; }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                for (int i = 0; i < 10; i++)
                {
                    Images.Add(new ImageDto { });
                }

                CurrentImage = "http://" + App.baseUrl + "/uploads/_MG_7004.jpg";

            }
            else
            {
                // Code runs "for real"
                OnImageTapCommand = new RelayCommand<string>(OnImageTap);
            }
        }


        ObservableCollection<ImageDto> _images = new ObservableCollection<ImageDto>();

        public ObservableCollection<ImageDto> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                this.RaisePropertyChanged("Images");
            }
        }


        string _currentImage;

        public string CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                this.RaisePropertyChanged("CurrentImage");
            }
        }

        private void OnImageTap(string image)
        {
            CurrentImage = image;
       
        }


        
    }
}