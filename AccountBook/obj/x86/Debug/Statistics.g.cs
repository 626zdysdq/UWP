﻿#pragma checksum "C:\Users\Lenovo\Desktop\AccountBookV2 (4)\AccountBookV2 (3)\AccountBook\Statistics.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A19FFD854B735F251102A92698CB6330"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccountBook
{
    partial class Statistics : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // Statistics.xaml line 40
                {
                    this.TypePie = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 2: // Statistics.xaml line 82
                {
                    this.MonthBar = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // Statistics.xaml line 87
                {
                    this.bar = (global::Telerik.UI.Xaml.Controls.Chart.RadCartesianChart)(target);
                }
                break;
            case 4: // Statistics.xaml line 53
                {
                    this.PieChart = (global::Telerik.UI.Xaml.Controls.Chart.RadPieChart)(target);
                }
                break;
            case 5: // Statistics.xaml line 50
                {
                    this.receipt = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.receipt).Checked += this.receipt_Checked;
                }
                break;
            case 6: // Statistics.xaml line 51
                {
                    this.pay = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.pay).Checked += this.pay_Checked;
                }
                break;
            case 7: // Statistics.xaml line 27
                {
                    this.Text1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8: // Statistics.xaml line 28
                {
                    this.DateEnd = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                }
                break;
            case 9: // Statistics.xaml line 29
                {
                    this.Text2 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10: // Statistics.xaml line 30
                {
                    this.DateScope = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 11: // Statistics.xaml line 35
                {
                    this.Text3 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12: // Statistics.xaml line 36
                {
                    this.TypeComparison = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.TypeComparison).Click += this.TypeComparison_Click;
                }
                break;
            case 13: // Statistics.xaml line 37
                {
                    this.MonthComparison = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.MonthComparison).Click += this.MonthComparison_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

