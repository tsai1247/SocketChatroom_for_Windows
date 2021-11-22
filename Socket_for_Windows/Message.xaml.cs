using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Socket_for_Windows
{
    /// <summary>
    /// Message.xaml 的互動邏輯
    /// </summary>
    public partial class Message : UserControl
    {
        public Message()
        {
            InitializeComponent();
        }

        public Message(string nickyName, string curText, string curTime)
        {
            InitializeComponent();

            name.Text = nickyName;
            content.Text = curText;
            timestamp.Text = curTime;
        }
    }
}
