using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AccountBook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();


            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string BGM = (string)localSettings.Values["BGM"];
            if (BGM == null) BGM = "默认BGM";
            bgmName.Text = "当前BGM：" + BGM;
        }

        //打开媒体文件
        private async void openMedia_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker mediaPicker = new FileOpenPicker();
            mediaPicker.FileTypeFilter.Add(".mp3");

            mediaPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;

            var file = await mediaPicker.PickSingleFileAsync();

            if (file != null)
            {
                //设置媒体播放源
                IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
                MediaSource mediaSource = MediaSource.CreateFromStorageFile(file);
                MainPage.Current.setMediaSource(ir, " ");

                string type = file.FileType;
                string userBGM;

                //保存音乐文件名
                userBGM = file.Name;
                try
                {
                    //由该音乐创建拥有相同名字的新文件，存放在LocalFolder
                    await file.CopyAsync(ApplicationData.Current.LocalFolder, userBGM);
                }
                catch (Exception ee)
                {
                    //已经有同名文件存在，则替换
                    await file.CopyAndReplaceAsync(await ApplicationData.Current.LocalFolder.GetFileAsync(userBGM));
                }

                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["BGM"] = userBGM;

                bgmName.Text = "当前BGM：" + userBGM;
            }

        }

        //重置默认BGM
        private void resetMedia_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["BGM"] = null;
            bgmName.Text = "当前BGM：默认BGM";
            MainPage.Current.setMediaDefault();
        }

        //设置音量
        private void volumnSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                MainPage.Current.setMediaVolume(slider.Value / 100);
            }
        }
    }
}
