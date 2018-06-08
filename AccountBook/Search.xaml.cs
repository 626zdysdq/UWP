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
    public sealed partial class Search : Page
    {
        public static string word;      //搜索关键词
        public static Search Current;
        public Search()
        {
            this.InitializeComponent();
            Current = this;

            Tip.Text = "\"" + word + "\"的搜索结果：";

            if (word != "") SearchResult.Text = ViewModels.BookViewModel.Instance.Search(word).ToString();
            else SearchResult.Text = "无";
        }
    }
}
