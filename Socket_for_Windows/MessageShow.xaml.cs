using System.Windows.Controls;

namespace Socket_for_Windows
{
    /// <summary>
    /// Message.xaml 的互動邏輯
    /// </summary>
    public partial class MessageShow : UserControl
    {
        public MessageShow()
        {
            InitializeComponent();
        }

        public MessageShow(string nickyName, string curText, string curTime)
        {
            InitializeComponent();

            name.Text = nickyName;
            content.Text = curText;
            timestamp.Text = curTime;
        }
    }
}
