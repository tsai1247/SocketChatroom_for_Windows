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
        internal static Address currentAddress;
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
        internal static void ClearRoom()
        {
            while ((Application.Current.MainWindow as MainWindow).Room.MessageContainer.Children.Count > 0)
                (Application.Current.MainWindow as MainWindow).Room.MessageContainer.Children.RemoveAt(0);
        }
        internal static void RestoreRoom(Address address)
        {
            currentAddress = address;
            foreach(var message in General.roomStorage[currentAddress])
            {
                AddMessage(message);
            }
        }


    }
}

