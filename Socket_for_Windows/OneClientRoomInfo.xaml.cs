using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        Address server_address;
        private void UserControl_Loaded()
        {
            new Thread(() =>
            {
                server_address =  STAGetAddress();

                General.roomStorage.Add(server_address, new List<Message>());
                Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Address local_address = Address.GetRandomPort();
                server_socket.Bind(Network.ToIPEndPoint(local_address));
                server_socket.Connect(new IPEndPoint(IPAddress.Parse(server_address.Host), int.Parse(server_address.Port)));
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
                    try
                    {
                        Message message = JsonConvert.DeserializeObject<Message>(ret);
                        SwitchToSTA_ShowMessage(message);
                    }
                    catch
                    {
                        SwitchToSTA_ShowMessage(new Message("error", "errorMessage", DateTime.Now));
                    }
                }
            }

        }
        private void SwitchToSTA_ShowMessage(Message message)
        {
            General.roomStorage[server_address].Add(message);
            if (server_address != Room.currentAddress)
                return;

            this.Dispatcher.Invoke((Action)(() =>
            {
                Room.AddMessage(message);;
                var data = JsonConvert.SerializeObject(new Message("name", "content", DateTime.Now));
                Clipboard.SetText(data);
            }));
        }

        private Address STAGetAddress()
        {
            Address address = new Address("-1", "-1");
            this.Dispatcher.Invoke((Action)(() =>
            {
                address.Host = IP.Text;
                address.Port = Port.Text;
            }));

            return address;
        }

        private void enterRoom_Click(object sender, RoutedEventArgs e)
        {
            Room.ClearRoom();
            Room.RestoreRoom(server_address);
        }
    }
}
