using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Socket_for_Windows
{
    /// <summary>
    /// ChatroomList.xaml 的互動邏輯
    /// </summary>
    public partial class ChatroomList : UserControl
    {
        public ChatroomList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void add_Click(object sender, RoutedEventArgs e)
        {
            Address address = Address.GetRandomPort();
            AddNewRoom(address);
            nickyName.IsEnabled = false;
        }

        internal void AddNewRoom(Address address, OneClientRoomInfo oneClientRoomInfo = null)
        {
            if (oneClientRoomInfo == null)
            {
                OneRoomInfo oneRoomInfo = new OneRoomInfo(address);
                roomList.Children.Add(oneRoomInfo);
            }
            else
            {
                roomList.Children.Add(oneClientRoomInfo);
            }
            Network.addressList.Add(address);
        }

        private void nickyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nickyName != null && nickyName.Text != null && nickyName.Text != "")
            {
                add.IsEnabled = true;
                General.GetMainWindow().joinGird.Join.IsEnabled = true;
                nickyHint.Visibility = Visibility.Hidden;
            }
            else
            {
                add.IsEnabled = false;
                General.GetMainWindow().joinGird.Join.IsEnabled = false;
                nickyHint.Visibility = Visibility.Visible;
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
                nickyHint.Foreground = Transfer.Color2Brush(Color.FromRgb(0, 0, 0));
            }
            else
            {
                nickyHint.Foreground = Transfer.Color2Brush(Color.FromRgb(255, 255, 255));
            }

            if (sender != null)
                mainGrid.Children.Remove(sender as ColorPicker);
            isChangingBG = false;

            if (sender != null)
            {
                SqliteConnection sql = new SqliteConnection("Data Source = Database.db");
                sql.Open();
                var cur = sql.CreateCommand();
                cur.CommandText = "Update Background set Color = @para1 where Name = 'chatroomlist'";
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
