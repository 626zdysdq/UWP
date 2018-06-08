using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBook.ViewModels
{
    public class AccountViewModel
    {
        public static AccountViewModel Instance
        {
            get
            {
                if (instance == null) instance = new AccountViewModel();
                return instance;
            }
        }
        private static AccountViewModel instance;                          //ViewModel的单例

        private Models.Account selectedAccount = default(Models.Account);  //当前选中的账目
        public Models.Account SelectedAccount { get { return selectedAccount; } set { this.selectedAccount = value; } }

        private ObservableCollection<Models.Account> allAccounts = new ObservableCollection<Models.Account>();    //当前视图中应该显示的账目
        public ObservableCollection<Models.Account> AllAccounts { get { return this.allAccounts; } }

        //添加一条账目
        public void AddAccount(DateTime time,bool receiptOrPay,string type,double money,string remark)
        {
            Models.Account toAdd = new Models.Account(time,receiptOrPay,type,money,remark);
            allAccounts.Add(toAdd);
            //在sql里面插入该条账目
            BookViewModel.Instance.InsertItem(toAdd);
        }
        //在新的视图中添加要显示的账目
        public void AddAccount(Models.Account account)
        {
            allAccounts.Add(account);
        }
        //清空要显示的账目
        public void RefreshAccount()
        {
            allAccounts.Clear();
        }
        //更新账目
        public void UpdateAccount(DateTime time, bool receiptOrPay, string type, double money, string remark)
        {
            if (selectedAccount != null)
            {
                selectedAccount.time = time;
                selectedAccount.receiptOrPay = receiptOrPay;
                selectedAccount.type = type;
                selectedAccount.money = money;
                selectedAccount.remark = remark;
                //sql更新账目信息
                BookViewModel.Instance.UpdateItem(selectedAccount);
            }
            selectedAccount = null;
        }
        //删除账目
        public void DeleteAccount()
        {
            //sql删除一条账目信息
            BookViewModel.Instance.DeleteItem(selectedAccount);
            allAccounts.Remove(selectedAccount);
            selectedAccount = null;
        }
    }
}
