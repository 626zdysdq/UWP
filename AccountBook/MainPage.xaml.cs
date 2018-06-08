using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace AccountBook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        
        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
            ContentFrame.Navigate(typeof(BookAccount));

            //加载背景音乐
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string bgm = localSettings.Values["BGM"] as string;
            if (bgm != null)
            {
                Uri uri = new Uri("ms-appdata:///local/" + bgm);
                mediaPlayer.Source = uri;
            }

            //更新动态磁贴
            Services.TileService.UpdatePrimaryTileByFormat();

        }

        //点击导航菜单时触发
        private void NaView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(Settings));    //转到设置页
            }
            else
            {
                //导航项的内容
                switch (args.InvokedItem)
                {
                    case "我的账本":
                        ContentFrame.Navigate(typeof(BookAccount));
                        break;
                    case "汇率查询":
                        ContentFrame.Navigate(typeof(RateInquiry));
                        break;
                    case "暂停音乐":
                        mediaPlayer.Pause();
                        ((SymbolIcon)BgmNav.Icon).Symbol = Symbol.Play;
                        BgmNav.Content = "播放音乐";
                        break;
                    case "播放音乐":
                        mediaPlayer.Play();
                        ((SymbolIcon)BgmNav.Icon).Symbol = Symbol.Pause;
                        BgmNav.Content = "暂停音乐";
                        break;
                    default: break;
                }
            }
        }

        //打开统计页面
        public void GotoStatistic()
        {
            ContentFrame.Navigate(typeof(Statistics));
        }


        public void setMediaDefault()
        {
            Uri uri = new Uri("ms-appx:///Assets/defaultBGM.mp3");
            mediaPlayer.Source = uri;
        }

        //设置BGM播放流
        public void setMediaSource(IRandomAccessStream ir, string str)
        {
            mediaPlayer.SetSource(ir, str);
        }

        //播放结束后的重新开始播放
        private void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Position = new TimeSpan(0, 0, 0);
            mediaPlayer.Play();
        }

        //设置音量
        public void setMediaVolume(double volume)
        {
            mediaPlayer.Volume = volume;
        }

        //搜索
        private void ASB_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string txt = args.QueryText;
            if(args.ChosenSuggestion != null)
            {

            }
            else
            {
                Search.word = txt;
                ContentFrame.Navigate(typeof(Search));
            }
        }
    }
}
