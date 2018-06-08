using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
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
    public sealed partial class EditAccount : Page
    {
        private ViewModels.AccountViewModel AccountViewModel;       //AccountViewModel，用于更改选中的账目
        private ViewModels.BookViewModel BookViewModel;             //BookViewModel，用于查看选中的账本
        bool isPay = false;                                         //该条账目是否是支出
        public static EditAccount Current;

        public EditAccount()
        {
            this.InitializeComponent();
            this.AccountViewModel = ViewModels.AccountViewModel.Instance;
            this.BookViewModel = ViewModels.BookViewModel.Instance;
            Current = this;
        }
        //取消按钮点击,返回
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
        //创建或更新一条账目
        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            bool before = BookViewModel.SelectedBook.CheckMonthlyBudget(date.Date.Year, date.Date.Month);
            if (await Check(money, remark, date))
            {
                string typeItem = "一般";
                if(isPay)
                {
                    var item = (ComboBoxItem)payType.SelectedItem;
                    //将类型变成字符串
                    typeItem = item.Content.ToString();
                }
                else
                {
                    var item = (ComboBoxItem)receiptType.SelectedItem;
                    //将类型变成字符串
                    typeItem = item.Content.ToString();
                }
                //将文本转化为double类型
                double dmoney = Convert.ToDouble(money.Text);
                dmoney = Math.Round(dmoney, 2);
                if ((string)createButton.Content == "创建")
                {
                    //视图中增加一条账目
                    AccountViewModel.AddAccount(date.Date.DateTime, isPay, typeItem, dmoney, remark.Text);
                    //选中的账本增加一条账目
                    BookViewModel.SelectedBook.AddAccount(date.Date.DateTime, isPay, typeItem, dmoney, remark.Text);
                }
                else
                {
                    //选中的账本更新该条账目信息
                    BookViewModel.SelectedBook.UpdateAccount(AccountViewModel.SelectedAccount,date.Date.DateTime, isPay, typeItem, dmoney, remark.Text);
                    //视图中更新该条账目
                    AccountViewModel.UpdateAccount(date.Date.DateTime, isPay, typeItem, dmoney, remark.Text);
                }
                Frame.GoBack();

                bool after = BookViewModel.SelectedBook.CheckMonthlyBudget(date.Date.Year, date.Date.Month);
                if(date.Date.DateTime.Year == DateTime.Now.Year && date.Date.DateTime.Month == DateTime.Now.Month && before && !after)
                {   
                    //如果是当前月，则检查是否超支
                    ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(Services.ToastService.CreateToast()));
                }

                //更新磁贴
                Services.TileService.UpdatePrimaryTileByFormat();

            }
        }
        //检测所有信息是否合法
        public static async Task<bool> Check(TextBox money, TextBox remark, DatePicker date)
        {
            bool haveError = false;
            var errorMes = "";
            if (money.Text.Length == 0)
            {
                haveError = true;
                errorMes += "金额不能为空。\n";
            }
            else
            {
                //使用正则匹配浮点数
                if(!Regex.IsMatch(money.Text.ToString(), @"^(\d+)(\.\d+)?$"))
                {
                    haveError = true;
                    errorMes += "所输入的金额不是合法的数字。\n";
                }
            }
            /* 备注允许为空
            if (remark.Text.Length == 0)
            {
                haveErorr = true;
                erorrMes += "The remark cannot be empty.\n";
            }
            */
            //日期不能超过当前日期
            bool dataIsCorrect = false;
            if (date.Date.Year < DateTime.Now.Date.Year ||
                date.Date.Year == DateTime.Now.Date.Year && date.Date.Month < DateTime.Now.Date.Month ||
                date.Date.Year == DateTime.Now.Date.Year && date.Date.Month == DateTime.Now.Date.Month && date.Date.Day <= DateTime.Now.Date.Day)
            {
                dataIsCorrect = true;
            }
            if(!dataIsCorrect)
            {
                haveError = true;
                errorMes += "时间不能超过当前日期。\n";
            }
            if (haveError)
            {
                var msgDialog = new Windows.UI.Popups.MessageDialog(errorMes) { Title = "" };
                await msgDialog.ShowAsync();
                return false;
            }
            else
            {
                var msgDialog = new Windows.UI.Popups.MessageDialog(EditAccount.Current.createButton.Content + "成功！") { Title = "" };
                await msgDialog.ShowAsync();
                return true;
            }
        }
        //收被点击
        private void receipt_Checked(object sender, RoutedEventArgs e)
        {
            isPay = false;
        }
        //支被点击
        private void pay_Checked(object sender, RoutedEventArgs e)
        {
            isPay = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                AccountViewModel = ViewModels.AccountViewModel.Instance;
                if (AccountViewModel == null || AccountViewModel.SelectedAccount == null)
                {
                    //新建一条账目
                    createButton.Content = "创建";
                }
                else
                {
                    //更新一条账目
                    createButton.Content = "修改";
                    //将对应信息显示在编辑账目界面
                    if(AccountViewModel.SelectedAccount.receiptOrPay)
                    {
                        pay.IsChecked = true;
                        var item = (ComboBoxItem)payType.SelectedItem;
                        item.Content = (object)AccountViewModel.SelectedAccount.type;
                    }
                    else
                    {
                        receipt.IsChecked = true;
                        var item = (ComboBoxItem)receiptType.SelectedItem;
                        item.Content = (object)AccountViewModel.SelectedAccount.type;
                    }

                    money.Text = AccountViewModel.SelectedAccount.money.ToString();
                    remark.Text = AccountViewModel.SelectedAccount.remark;
                    date.Date = AccountViewModel.SelectedAccount.time;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //更新每条账目信息
            foreach (Models.Account item in AccountViewModel.AllAccounts)
                BookViewModel.UpdateItem(item);
        }
    }
}
