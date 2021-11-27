using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var host = Network.localHost.Split(".");
            Host.Text = Network.localHost;
            //Host.Text = string.Format("{0}.{1}.", host[0], host[1]);
        }
    }
}
