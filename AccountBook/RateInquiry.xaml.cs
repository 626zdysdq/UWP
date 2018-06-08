using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
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
    public sealed partial class RateInquiry : Page
    {
        public RateInquiry()
        {
            this.InitializeComponent();
        }

        //币种汇率查询，使用Xml格式
        private async void Query(object sender, RoutedEventArgs e)
        {
            ComboBoxItem srcItem = ((ComboBoxItem)srcCurrency.SelectedItem);
            String src = (String)srcItem.Content;
            ComboBoxItem dstItem = ((ComboBoxItem)dstCurrency.SelectedItem);
            String dst = (String)dstItem.Content;
            src = src.Substring(src.Length - 3);
            dst = dst.Substring(dst.Length - 3);

            String appkey = "34070";
            String sign = "dda69d61811355e1161350a3ee325eb3"; 
            String format = "http://api.k780.com/?app=finance.rate&scur=" + src + "&tcur=" + dst +
                "&appkey=" + appkey + "&sign=" + sign + "&format=xml";

            //访问API
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(format);
            request.Method = "GET";
            request.ContentType = "application/xml";

            //读取数据
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            string xmlResult = streamReader.ReadToEnd();
            stream.Dispose();
            streamReader.Dispose();

            //解析Xml
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlResult);
            XmlNode root = xmlDocument.ChildNodes[1];
            if (root.ChildNodes[0].InnerText == "0")
            {
                rate.Text = "Error";
                return;
            }
            XmlNode result = root.ChildNodes[1];
            rate.Text = result.ChildNodes[4].InnerText;
        }
    }
}
