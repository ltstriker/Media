using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Media2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //MediaTimelineController _mediaTimelineController;
        public MainPage()
        {
            this.InitializeComponent();
        }

        public void Full(object sender, RoutedEventArgs e)
        {
            try
            {
                var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
                if (appView.IsFullScreenMode)
                {
                    appView.ExitFullScreenMode();
                    Full_Button.Content = "Full";
                }
                else
                {
                    appView.TryEnterFullScreenMode();
                    Full_Button.Content = "Exit Full";
                }
            }
            catch (Exception)
            {
                //
            }
        }

        public void Go(object sender, RoutedEventArgs e)
        {
            _mediaElement.Play();
            EllStoryboard.Begin();
        }

        public void Stop(object sender, RoutedEventArgs e)
        {
            _mediaElement.Pause();
            EllStoryboard.Pause();
        }

        public void Reset(object sender, RoutedEventArgs e)
        {
            _mediaElement.Stop();
            EllStoryboard.Stop();
        }

        public async void New(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".wmv");
                openPicker.FileTypeFilter.Add(".mp4");
                openPicker.FileTypeFilter.Add(".wma");
                openPicker.FileTypeFilter.Add(".mp3");

                StorageFile file = await openPicker.PickSingleFileAsync();
                if(file != null)
                {
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    _mediaElement.SetSource(stream, file.ContentType);
                    _mediaElement.Play();
                    if (file.FileType == ".mp3")
                    {
                        //_mediaElement.PosterSource = new BitmapImage(new Uri("ms-appx:///Assets/menkey.jpg"));
                        StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 500, ThumbnailOptions.ResizeThumbnail);
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.SetSource(thumbnail);
                        picture.ImageSource = bitmap;
                        EllStoryboard.Begin();
                        ellipse.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ellipse.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GetSliderCount(object sender, RoutedEventArgs e)
        {
            timeLine.Maximum = _mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
        }
    }
}
