using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class ListBook : Page
    {
        public static ListBook Current;                      //ListBook的实例，用于在其他界面调用自己
        private ViewModels.BookViewModel BookViewModel;      //BookViewModel,用于更改选中的账本
        public ListBook()
        {
            this.InitializeComponent();
            this.BookViewModel = ViewModels.BookViewModel.Instance;
            Current = this;
        }

        //添加一个账本，转到编辑界面
        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            BookViewModel.SelectedBook = null;
            Frame.Navigate(typeof(EditBook));
        }

        //选中一个账本
        private void bookListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            BookViewModel.SelectedBook = (Models.Book)(e.ClickedItem);
            //打开账目页，账目界面刷新为当前账本的账目信息
            BookAccount.Current.OpenAccount();

        }

        //编辑一个账本
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            //设置当前选中账本
            MenuFlyoutItem selectedBook = sender as MenuFlyoutItem;
            BookViewModel.SelectedBook = selectedBook.DataContext as Models.Book;
            //转到编辑界面
            Frame.Navigate(typeof(EditBook));
        }

        //删除一个账本
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem selectedBook = sender as MenuFlyoutItem;
            BookViewModel.SelectedBook = selectedBook.DataContext as Models.Book;
            //在视图中删除该账本
            BookViewModel.DeleteBook();
            Frame.Navigate(typeof(ListBook));

            BookViewModel.SelectedBook = null;
            ListAccount.Current.refresh();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //刷新账目列表
            if (ListAccount.Current != null)
                ListAccount.Current.refresh();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //更新所有账本信息
            foreach (Models.Book item in BookViewModel.AllBooks)
                BookViewModel.UpdateItem(item);
        }

        //该账本的统计数据
        private void statistics_Click(object sender, RoutedEventArgs e)
        {
            //设置当前选中账本
            MenuFlyoutItem selectedBook = sender as MenuFlyoutItem;
            BookViewModel.SelectedBook = selectedBook.DataContext as Models.Book;

            MainPage.Current.GotoStatistic();
        }
    }
}
