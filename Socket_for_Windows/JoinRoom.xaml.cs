﻿using System.Windows;
using System.Windows.Controls;

namespace Socket_for_Windows
{
    /// <summary>
    /// JoinRoom.xaml 的互動邏輯
    /// </summary>
    public partial class JoinRoom : UserControl
    {
        public JoinRoom()
        {
            InitializeComponent();
        }
        
        private void Join_Click(object sender, RoutedEventArgs e)
        {
            Address address = new Address(Host.Text, Port.Text);
            OneClientRoomInfo oneClientRoomInfo = new OneClientRoomInfo(address);
            General.GetMainWindow().chatRoomList.AddNewRoom(address, oneClientRoomInfo);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var host = Network.localHost.Split(".");
            Host.Text = Network.localHost;
            //Host.Text = string.Format("{0}.{1}.", host[0], host[1]);
        }
    }
}
