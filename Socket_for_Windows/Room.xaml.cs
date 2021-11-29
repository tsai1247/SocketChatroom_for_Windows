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
            tmp.content.Text = "aaaa\nbbbb\ncccc\ndddd\neeee\nffff";
            currentAddress.Host = null;
            currentAddress.Port = null;

        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (currentAddress.Host == null) return;

            string curText = text_for_send.Text;
            if (curText.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "") == "") return;
            text_for_send.Text = "";

            Message currentMessage = new Message(User.NickyName, curText, DateTime.Now);
            SendMessage(currentMessage);
            AddMessage(currentMessage);
            
        }

        internal static Message SendMessage(Message message)
        {
            if(General.serverRoom.ContainsKey(currentAddress))
            {
                General.serverRoom[currentAddress].messageQueue.Enqueue(message);
            }
            else if(General.clientRoom.ContainsKey(currentAddress))
            {
                General.clientRoom[currentAddress].messageQueue.Enqueue(message);
            }
            return message;
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
            if (General.roomStorage.ContainsKey(currentAddress))
            {
                foreach (var message in General.roomStorage[currentAddress])
                {
                    AddMessage(message);
                }
            }
        }


    }
}

