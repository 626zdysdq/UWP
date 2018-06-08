using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBook.Models
{
    public class Account
    {
        public string id { get; set; }           //唯一标示
        public DateTime time { get; set; }       //日期
        public bool receiptOrPay { get; set; }   //收或支出
        public string type { get; set; }         //类型
        public double money { get; set; }        //金额
        public string remark { get; set; }       //备注

        public Account(DateTime time,bool receiptOrPay,string type,double money,string remark, string id = "")
        {
            if (id == "")
                this.id = Guid.NewGuid().ToString();
            else
                this.id = id;
            this.time = time;
            this.receiptOrPay = receiptOrPay;
            this.type = type;
            this.money = money;
            this.remark = remark;
        }
    }
}
