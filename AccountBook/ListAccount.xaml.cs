using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ListAccount : Page
    {
        public static ListAccount Current;                     //ListAccount实例，用于在其他页面调用自己
        private ViewModels.BookViewModel BookViewModel;        //BookViewModel,用于得到当前选中的账本
        private ViewModels.AccountViewModel AccountViewModel;  //AccountViewModel，用于更改当前选中的账目

        public ListAccount()
        {
            this.InitializeComponent();
            this.BookViewModel = ViewModels.BookViewModel.Instance;
            this.AccountViewModel = ViewModels.AccountViewModel.Instance;
            Current = this;
        }
        //需要创建新的账目，转到编辑账目页面
        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            AccountViewModel.SelectedAccount = null;
            Frame.Navigate(typeof(EditAccount));
        }
        //删除一条账目
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            //设置当前选中的账目
            MenuFlyoutItem selectedAccount = sender as MenuFlyoutItem;
            AccountViewModel.SelectedAccount = selectedAccount.DataContext as Models.Account;
            //选中的账本删除该条账目
            BookViewModel.SelectedBook.DeleteAccount(AccountViewModel.SelectedAccount);
            //在视图中删除该条账目
            AccountViewModel.DeleteAccount();


            //更新动态磁贴
            Services.TileService.UpdatePrimaryTileByFormat();

            Frame.Navigate(typeof(ListAccount));
        }

        //分享一条账目
        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            //设置当前选中的账目
            MenuFlyoutItem selectedItem = sender as MenuFlyoutItem;
            AccountViewModel.SelectedAccount = selectedItem.DataContext as Models.Account;
            //分享界面
            DataTransferManager.ShowShareUI();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //绑定分享事件
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            if(BookViewModel.SelectedBook != null)
            {
                //选中了一个账本，需要清空当前视图中的账目
                AccountViewModel.RefreshAccount();
                //将选中账本中的账目添加到视图中
                foreach (Models.Account element in BookViewModel.SelectedBook.accounts)
                {
                    AccountViewModel.AddAccount(element);
                }
                //视图可见
                scrollviewer.Visibility = Visibility.Visible;
                addbutton.Visibility = Visibility.Visible;
            }
            else
            {
                //视图界面不可见
                scrollviewer.Visibility = Visibility.Collapsed;
                addbutton.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
            //更新视图中的账目信息
            foreach (Models.Account item in AccountViewModel.AllAccounts)
                BookViewModel.UpdateItem(item);
        }

        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            bool pay = AccountViewModel.SelectedAccount.receiptOrPay;
            //标题 = 时间 支出/收入 类型 金额
            string title = "";
            title = AccountViewModel.SelectedAccount.time.ToString() + " ";
            if (pay)
                title += "支出 ";
            else
                title += "收入 ";
            title += AccountViewModel.SelectedAccount.type + " ";
            title += AccountViewModel.SelectedAccount.money.ToString();
            request.Data.Properties.Title = title;
            //描述为账目的备注
            request.Data.Properties.Description = AccountViewModel.SelectedAccount.remark;
            //图片为默认图片
            request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/background.jpg")));
        }

        //刷新该界面
        public void refresh()
        {
            Frame.Navigate(typeof(ListAccount));
        }

        //转到编辑账目页面
        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //设置当前选中账目
            AccountViewModel.SelectedAccount = (Models.Account)(e.ClickedItem);
            Frame.Navigate(typeof(EditAccount));
        }
    }
}
