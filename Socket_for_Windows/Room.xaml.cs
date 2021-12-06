using Mono.Data.Sqlite;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Socket_for_Windows
{
    /// <summary>
    /// Room.xaml 的互動邏輯
    /// </summary>
    public partial class Room : UserControl
    {
        internal static Address currentAddress;
        internal static object currentRoom;
        public Room()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            currentAddress.Host = null;
            currentAddress.Port = null;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (currentAddress.Host == null) return;

            string curText = text_for_send.Text;
            if (curText.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "") == "") return;
            text_for_send.Text = "";

            Message currentMessage = new Message(General.GetMainWindow().chatRoomList.nickyName.Text, curText, DateTime.Now);
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
            General.roomStorage[currentAddress].Add(message);
            return message;
        }

        internal static Message AddMessage(Message message)
        {
            MessageShow messageShow = new MessageShow(message.nickyName, message.content, message.dateTime.ToString("t"));
            if (General.members[currentAddress].Contains(message.nickyName))
            {
                messageShow.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                messageShow.HorizontalAlignment = HorizontalAlignment.Right;
            }

            General.GetMainWindow().Room.MessageContainer.Children.Add(messageShow);
            General.GetMainWindow().Room.scroll.ScrollToEnd();

            return message;
        }
        internal static void ClearRoom()
        {
            while ((Application.Current.MainWindow as MainWindow).Room.MessageContainer.Children.Count > 0)
                (Application.Current.MainWindow as MainWindow).Room.MessageContainer.Children.RemoveAt(0);
        }

        
        internal static void RestoreRoom(object room, Address address)
        {
            currentRoom = room;
            currentAddress = address;
            if (General.roomStorage.ContainsKey(currentAddress))
            {
                for(int i=0; i<General.roomStorage[currentAddress].Count; i++)
                {
                        AddMessage(General.roomStorage[currentAddress][i]);
                }
            }
        }

        bool isChangingBG = false;
        private void Background_Click(object sender, RoutedEventArgs e)
        {
            if (!isChangingBG)
            {
                isChangingBG = true;
                ColorPicker color = new ColorPicker();
                color.Width = 60;
                color.Height = 50;
                color.FontSize = 25;
                color.SelectedColor = Transfer.Brush2Color(mainGrid.Background);
                color.SelectedColorChanged += Color_SelectedColorChanged;
                mainGrid.Children.Add(color);
            }
        }

        private void Color_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color color = (Color)(sender as ColorPicker).SelectedColor;
            ChangeColor(sender, color);

        }

        public void ChangeColor(object sender, Color color)
        {
            mainGrid.Background = Transfer.Color2Brush(color);
            if (IsDark(color))
            {
                chooseHint.Foreground = Transfer.Color2Brush(Color.FromRgb(0, 0, 0));
            }
            else
            {
                chooseHint.Foreground = Transfer.Color2Brush(Color.FromRgb(255, 255, 255));
            }

            if(sender != null)
                mainGrid.Children.Remove(sender as ColorPicker);
            isChangingBG = false;

            if (sender != null)
            {
                SqliteConnection sql = new SqliteConnection("Data Source = Database.db");
                sql.Open();
                var cur = sql.CreateCommand();
                cur.CommandText = "Update Background set Color = @para1 where Name = 'room'";
                cur.Parameters.AddWithValue("@para1", color);
                cur.ExecuteNonQuery();
                sql.Close();
            }
        }

        float bias = 0.9f;
        private bool IsDark(Color color)
        {
            return (color.R + color.G + color.B) > 255 * 3 / 2 * bias;
        }
    }
}

