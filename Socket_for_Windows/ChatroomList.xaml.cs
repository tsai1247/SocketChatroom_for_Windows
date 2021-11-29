using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

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
    }
}
