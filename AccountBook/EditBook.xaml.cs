using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    public sealed partial class EditBook : Page
    {
        public static EditBook Current;
        private ViewModels.BookViewModel BookViewModel;            //BookViewModel，用于更改选中的账本
        public EditBook()
        {
            this.InitializeComponent();
            Current = this;
            this.BookViewModel = ViewModels.BookViewModel.Instance;
        }
        //创建或更新账本信息
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (await Check())
            {
                double bu;
                if (budget.Visibility == Visibility.Visible)
                {
                    bu = Convert.ToDouble(budget.Text);
                    bu = Math.Round(bu, 2);
                }
                else bu = -1;
                if ((string)createButton.Content == "创建")
                    BookViewModel.AddBook(name.Text, bu);
                else
                    BookViewModel.UpdateBook(name.Text, bu);
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                BookViewModel = ViewModels.BookViewModel.Instance;
                if (BookViewModel == null || BookViewModel.SelectedBook == null)
                {
                    //创建账本
                    createButton.Content = "创建";
                    name.Text = "";
                }
                else
                {
                    //更新账本
                    createButton.Content = "修改";
                    name.Text = BookViewModel.SelectedBook.name;
                    budget.Text = BookViewModel.SelectedBook.monthlyBudget.ToString();
                    if (BookViewModel.SelectedBook.monthlyBudget < 0) limitless.IsChecked = true;
                    else limitted.IsChecked = true;
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //更新每个账本的信息
            foreach (Models.Book item in BookViewModel.AllBooks)
                BookViewModel.UpdateItem(item);
        }

        //检测所有信息是否合法
        public async Task<bool> Check()
        {
            if (budget.Visibility == Visibility.Collapsed) return true;
            bool haveError = false;
            var errorMes = "";
            if (budget.Visibility == Visibility.Visible && budget.Text.Length == 0)
            {
                haveError = true;
                errorMes += "金额不能为空。\n";
            }
            else
            {
                //使用正则匹配浮点数
                if (!Regex.IsMatch(budget.Text.ToString(), @"^(\d+)(\.\d+)?$"))
                {
                    haveError = true;
                    errorMes += "所输入的预算不是合法的数字。\n";
                }
            }
            if (haveError)
            {
                var msgDialog = new Windows.UI.Popups.MessageDialog(errorMes) { Title = "" };
                await msgDialog.ShowAsync();
                return false;
            }
            else
            {
                var msgDialog = new Windows.UI.Popups.MessageDialog(EditBook.Current.createButton.Content + "成功！") { Title = "" };
                await msgDialog.ShowAsync();
                return true;
            }
        }

        private void limitted_Checked(object sender, RoutedEventArgs e)
        {
            budget.Visibility = Visibility.Visible;
        }

        private void limitless_Checked(object sender, RoutedEventArgs e)
        {
            if (budget == null) return;
            budget.Visibility = Visibility.Collapsed;
            budget.Text = "-1";
        }
    }
}
