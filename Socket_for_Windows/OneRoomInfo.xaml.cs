using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

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
        Address targetAddress;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Socket socket_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress iPAddress = IPAddress.Parse(IP.Text);
            int port = int.Parse(Port.Text);

            socket_server.Bind(new IPEndPoint(iPAddress, port));

            int maxClient = 1;
            socket_server.Listen(maxClient);

            new Thread(() =>
            {
                for (int i = 0; i < maxClient; i++)
                {
                    Socket clientSocket = socket_server.Accept();
                    targetAddress = new Address(
                        (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString(),
                        (clientSocket.RemoteEndPoint as IPEndPoint).Port.ToString()
                    );

                    General.roomStorage.Add(targetAddress, new List<Message>());
                    new Thread(() => {
                        SwitchToSTA_ShowMessage(new Message("name", new Random().Next(1, 10000).ToString(), DateTime.Now));
                        SendAndReceive(ref clientSocket);
                    }).Start();
                }
            }).Start();


        }

        private void SendAndReceive(ref Socket clientSocket)
        {
            while (true)
            {
                byte[] strbyte = new byte[1024];
                int count = clientSocket.Receive(strbyte);
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
                        SwitchToSTA_ShowMessage( new Message("error", "errorMessage", DateTime.Now));
                    }
                }
            }
        }

        private void SwitchToSTA_ShowMessage(Message message)
        {
            this.Dispatcher.Invoke(() =>
            {
                Room.AddMessage(message);
                General.roomStorage[targetAddress].Add(message);
                var data = JsonConvert.SerializeObject(new Message("name", "content", DateTime.Now));
                Clipboard.SetText(data);
            });
        }

        private void enterRoom_Click(object sender, RoutedEventArgs e)
        {
            Room.ClearRoom();
            Room.RestoreRoom(targetAddress);
        }
    }
}
