﻿#pragma checksum "C:\Users\Lenovo\Desktop\AccountBookV2 (4)\AccountBookV2 (3)\AccountBook\EditBook.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3736FFA3C3B26D48E0C8117F87AC0118"
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
    partial class EditBook : 
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
            case 1: // EditBook.xaml line 20
                {
                    this.name = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 2: // EditBook.xaml line 26
                {
                    this.budget = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3: // EditBook.xaml line 28
                {
                    this.cancelButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.cancelButton).Click += this.cancelButton_Click;
                }
                break;
            case 4: // EditBook.xaml line 29
                {
                    this.createButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.createButton).Click += this.Button_Click;
                }
                break;
            case 5: // EditBook.xaml line 23
                {
                    this.limitless = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.limitless).Checked += this.limitless_Checked;
                }
                break;
            case 6: // EditBook.xaml line 24
                {
                    this.limitted = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.limitted).Checked += this.limitted_Checked;
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

