using System;
using System.Windows;
using System.Windows.Controls;

namespace Socket_for_Windows
{
    /// <summary>
    /// Room.xaml 的互動邏輯
    /// </summary>
    public partial class Room : UserControl
    {
        public Room()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string curText = text_for_send.Text;
            if (curText.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "") == "") return;
            text_for_send.Text = "";
            
            AddMessage( new Message( User.NickyName, curText, DateTime.Now));
            
        }


        internal static Message AddMessage(Message message)
        {
            MessageShow messageShow = new MessageShow(message.nickyName, message.content, message.dateTime.ToString("t"));

            (Application.Current.MainWindow as MainWindow).Room.MessageContainer.Children.Add(messageShow);
            return message;
        }

    }
}

