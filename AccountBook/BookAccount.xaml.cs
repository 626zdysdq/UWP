using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class BookAccount : Page
    {

        private const String stateAccountView = "AccountView";
        private const String stateBookView = "BookView";
        private const String stateAllView = "AllView";
        private String stateView = stateAllView;    //当前视图状态

        public static BookAccount Current;

        public BookAccount()
        {
            this.InitializeComponent();
            Current = this;

            //首页默认导航到ListBook和ListAccount
            left.Navigate(typeof(ListBook));
            right.Navigate(typeof(ListAccount));

            //打开时窗口宽度小于800，则显示账本页
            if (Window.Current.Bounds.Width < 800)
            {
                stateView = stateBookView;
                VisualStateManager.GoToState(this, stateView, false);
            }

            //由左右页的显示判断当前状态
            if (right.Visibility == Visibility.Collapsed) stateView = stateBookView;
            else if (left.Visibility == Visibility.Collapsed) stateView = stateAccountView;

            //注册窗口大小改变的事件
            this.SizeChanged += new SizeChangedEventHandler(Resize);

            Services.TileService.UpdatePrimaryTileByFormat();
        }

        //窗口大小改变时，改变左右两个子页的显示
        private void Resize(object sender, SizeChangedEventArgs e)
        {
            double neww = e.NewSize.Width;
            double oldw = e.PreviousSize.Width;
            if (neww >= 800) stateView = stateAllView;
            else
            {
                //如果窗口宽度减小到800以内，只显示账目
                if (oldw >= 800) stateView = stateAccountView;
            }
            VisualStateManager.GoToState(this, stateView, false);
        }

        //打开账目页
        public void OpenAccount()
        {
            //刷新账目页
            ListAccount.Current.refresh();
            if (Window.Current.Bounds.Width < 800)
            {
                stateView = stateAccountView;
                VisualStateManager.GoToState(this, stateView, false);
            }
        }
    }
}
