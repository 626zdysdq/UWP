using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;

namespace AccountBook.Services
{
    public class ToastService
    {
        public static XmlDocument CreateToast()
        {
            var xDoc = new XDocument(
                new XElement("toast",
                    new XElement("visual",
                        new XElement("binding", new XAttribute("template", "ToastGeneric"),
                            new XElement("text", "Kount记账"),
                            new XElement("text", "您这个月刚刚超支了哦！")
                            ) // binding
                        ), // visual
                    new XElement("actions",
                        new XElement("action", new XAttribute("activationType", "background"),
                            new XAttribute("content", "OK"), new XAttribute("arguments", "OK"))
                        ) // actions
                    )
                );

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            return xmlDoc;
        }
    }
}
