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
    public sealed partial class Statistics : Page
    {
        private Models.Book book = ViewModels.BookViewModel.Instance.SelectedBook;
        private bool receiveOrPay = false;      //要查看的是收入记录还是支出记录

        public Statistics()
        {
            this.InitializeComponent();
        }

        //收被选择，显示不同类型的收入饼状图
        private void receipt_Checked(object sender, RoutedEventArgs e)
        {
            if (PieChart == null) return;
            receiveOrPay = false;
            TypeComparison_Click(null, null);
        }

        //支被选择，显示不同类型的支出饼状图
        private void pay_Checked(object sender, RoutedEventArgs e)
        {
            if (PieChart == null) return;
            receiveOrPay = true;
            TypeComparison_Click(null, null);
        }

        //点击按钮产生收入/支出的各类型占比饼状图
        private void TypeComparison_Click(object sender, RoutedEventArgs e)
        {
            int offset = 0;
            ComboBoxItem item = (ComboBoxItem)DateScope.SelectedItem;
            switch (item.Content)
            {
                case ("一个月"):
                    offset = -1;
                    break;
                case ("三个月"):
                    offset = -3;
                    break;
                case ("六个月"):
                    offset = -6;
                    break;
                default:
                    break;
            }
            //获取时间范围的开端
            DateTimeOffset DateBegin = DateEnd.Date.AddMonths(offset);

            //为饼状图绑定数据
            PieChart.Series[0].ItemsSource = DataForTypeComparison(book, DateBegin, DateEnd.Date, receiveOrPay);

            //显示饼状图，不显示柱形图
            MonthBar.Visibility = Visibility.Collapsed;
            TypePie.Visibility = Visibility.Visible;
        }

        //点击按钮产生几个月内收支的柱形图
        private void MonthComparison_Click(object sender, RoutedEventArgs e)
        {
            int offset = 0;
            ComboBoxItem item = (ComboBoxItem)DateScope.SelectedItem;
            switch (item.Content)
            {
                case ("一个月"):
                    offset = -1;
                    break;
                case ("三个月"):
                    offset = -3;
                    break;
                case ("六个月"):
                    offset = -6;
                    break;
                default:
                    break;
            }
            DateTimeOffset begin = DateEnd.Date.AddMonths(offset + 1);
            begin = new DateTimeOffset(begin.Year, begin.Month, 1, 0, 0, 0, new TimeSpan());    //begin即时间范围内第一个月的第一天
            DateTimeOffset end = DateEnd.Date.AddMonths(1);
            end = new DateTimeOffset(end.Year, end.Month, 1, 0, 0, 0, new TimeSpan());          //end即时间范围内最后一个月的最后一天之后的第二天，也即时间范围后的第一天

            bar.DataContext = new TwoDatas(book, begin, end, -offset);

            MonthBar.Visibility = Visibility.Visible;
            TypePie.Visibility = Visibility.Collapsed;
        }

        //包括类型和金额，用于收支类型比较饼形图
        public class Data
        {
            public double Value { get; set; }
            public string Category { get; set; }
        }

        //包括月份和金额，用于月份比较柱形图
        public class MonthlyData
        {
            public double Value { get; set; }
            public string Month { get; set; }
        }

        //为收支类型比较饼形图提供数据列表
        public static List<Data> DataForTypeComparison(Models.Book book, DateTimeOffset begin, DateTimeOffset end, bool receiveOrPay)
        {
            List<Data> data = new List<Data>();

            foreach (var ac in book.accounts)
            {
                if (ac.time.CompareTo(begin.DateTime) < 0 || ac.time.CompareTo(end.DateTime) >= 0 || ac.receiptOrPay != receiveOrPay) continue;
                //Lambda表达式，表示Predicate<Data>委托
                if (!data.Exists(d => (d.Category == ac.type))) data.Add(new Data { Value = ac.money, Category = ac.type });
                else data.Find(d => (d.Category == ac.type)).Value += ac.money;
            }

            //排序，以便饼形图上不同大小的扇形可以有顺序地沿时针方向排列
            data.Sort(delegate (Data x, Data y)
            {
                return y.Value.CompareTo(x.Value);
            });

            foreach (var d in data)
            {
                d.Category = d.Category + " " + d.Value.ToString() + "元";
            }

            return data;
        }

        //为月份比较柱形图提供数据列表
        public static List<MonthlyData> DataForMonthComparison(Models.Book book, DateTimeOffset begin, DateTimeOffset end, int offset, bool receiveOrPay)
        {
            List<MonthlyData> data = new List<MonthlyData>();

            //记录要显示的几个月份
            for (int i = 0; i < offset; i++)
            {
                data.Add(new MonthlyData { Value = 0, Month = begin.Year.ToString("0000") + begin.Month.ToString("00") });
                begin = begin.AddMonths(1);
            }
            begin = begin.AddMonths(-offset);

            //记录月份对应的总收入/支出
            foreach(var ac in book.accounts)
            {
                //去掉不在当前所选时间范围，或有不同收支类型的记录
                if (ac.time.CompareTo(begin.DateTime) < 0 || ac.time.CompareTo(end.DateTime) >= 0 || ac.receiptOrPay != receiveOrPay) continue;
                string acMonth = ac.time.Year.ToString("0000") + ac.time.Month.ToString("00");
                data.Find(d => (d.Month.Equals(acMonth))).Value += ac.money;
            }

            foreach(var d in data)
            {
                d.Month = d.Month.Substring(d.Month.Length - 2) + "月";
            }
            return data;
        }

        public class TwoDatas
        {
            public IEnumerable<MonthlyData> Receive { get; private set; }   //每个月的收入记录
            public IEnumerable<MonthlyData> Pay { get; private set; }       //每个月的支出记录

            public TwoDatas(Models.Book book, DateTimeOffset begin, DateTimeOffset end, int offset)
            {
                Receive = DataForMonthComparison(book, begin, end, offset, false);
                Pay = DataForMonthComparison(book, begin, end, offset, true);
            }
        }
    }
}
