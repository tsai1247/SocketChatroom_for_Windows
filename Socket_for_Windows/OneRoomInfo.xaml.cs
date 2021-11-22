﻿using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace Socket_for_Windows
{
    /// <summary>
    /// OneRoomInfo.xaml 的互動邏輯
    /// </summary>
    public partial class OneRoomInfo : UserControl
    {
        public OneRoomInfo()
        {
            InitializeComponent();
        }

        public OneRoomInfo(Address address)
        {
            InitializeComponent();
            IP.Text = address.Host;
            Port.Text = address.Port;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void enterRoom_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
