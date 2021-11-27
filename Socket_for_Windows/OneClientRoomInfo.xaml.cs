using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Socket_for_Windows
{
    /// <summary>
    /// OneRoomInfo.xaml 的互動邏輯
    /// </summary>
    public partial class OneClientRoomInfo : UserControl
    {
        public OneClientRoomInfo()
        {
            InitializeComponent();
        }

        public OneClientRoomInfo(Address address)
        {
            InitializeComponent();
            IP.Text = address.Host;
            Port.Text = address.Port;
            UserControl_Loaded();
        }

        private void UserControl_Loaded()
        {
            new Thread(() =>
            {
                Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server_socket.Connect(new IPEndPoint(IPAddress.Parse(IP.Text), int.Parse(Port.Text)));
                new Thread(() =>
                {
                    SendAndReceive(ref server_socket);
                }).Start();
            }).Start();
        }

        private void SendAndReceive(ref Socket server_socket)
        {
            while (true)
            {
                byte[] strbyte = new byte[1024];
                int count = server_socket.Receive(strbyte);
                string ret = Encoding.UTF8.GetString(strbyte.SubArray(0, count));
                if (count > 0)
                {
                    // TODO: show it on screen
                }
            }

        }

        private void enterRoom_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
