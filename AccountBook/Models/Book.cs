using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBook.Models
{
    public class Book
    {
        public string id { get; set; }          //账本标识
        public string name { get; set; }        //账本名称
        public List<Account> accounts;          //账本中的账目

        public double monthlyBudget = -1;        //每月支出预算，-1表示无限制

        public Book(string name, double budget = -1, string id = "")
        {
            if (id == "")
                this.id = Guid.NewGuid().ToString();
            else
                this.id = id;
            this.name = name;
            this.monthlyBudget = budget;
            accounts = new List<Account>();
        }

        public void AddAccount(DateTime time, bool receiptOrPay, string type, double money, string remark,string id="")
        {
            Account toAdd = new Models.Account(time, receiptOrPay, type, money, remark,id);
            //加入一条账目
            accounts.Add(toAdd);
            //按照时间的降序排序
            accounts.Sort(delegate (Account x, Account y)
            {
                return y.time.Date.CompareTo(x.time.Date);
            });
        }

        public void DeleteAccount(Account selectedAccount)
        {
            //删除对应账目
            accounts.Remove(selectedAccount);
            //排序
            accounts.Sort(delegate (Account x, Account y)
            {
                return y.time.Date.CompareTo(x.time.Date);
            });
        }

        public void UpdateAccount(Account selectedAccount,DateTime time, bool receiptOrPay, string type, double money, string remark)
        {
            //移出该条账目
            accounts.Remove(selectedAccount);
            Account toAdd = new Models.Account(time, receiptOrPay, type, money, remark);
            //加入更新后的账目
            accounts.Add(toAdd);
            //排序
            accounts.Sort(delegate (Account x, Account y)
            {
                return y.time.Date.CompareTo(x.time.Date);
            });
        }

        public bool CheckMonthlyBudget(int year, int month)
        {
            //检查当月支出是否超出预算
            if (monthlyBudget < 0) return true;
            double monthlyPay = 0;
            foreach(var ac in accounts)
            {
                if (ac.receiptOrPay && ac.time.Year == year && ac.time.Month == month) monthlyPay += ac.money;
            }
            return monthlyBudget > monthlyPay;  //预算大于总支出，则返回true，否则返回false
        }
    }
}
