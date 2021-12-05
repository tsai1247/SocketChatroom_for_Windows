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
                General.activeSocket.Add(socket_server);
                for (int i = 0; i < maxClient; i++)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = socket_server.Accept();
                    }
                    catch
                    {
                        break;
                    }
                    targetAddress = new Address(
                        (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString(),
                        (clientSocket.RemoteEndPoint as IPEndPoint).Port.ToString()
                    );

                    General.serverRoom.Add(targetAddress, this);
                    General.roomStorage.Add(targetAddress, new List<Message>());
                    string nickyName = "";
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        nickyName = General.GetMainWindow().chatRoomList.nickyName.Text;
                    }));
                    clientSocket.Send(Encoding.ASCII.GetBytes("Name " + nickyName));
                    new Thread(() => {
                        Send(ref clientSocket);
                    }).Start();
                    new Thread(() => {
                        Receive(ref clientSocket);
                    }).Start();
                }
            }).Start();

        }

        public Queue<Message> messageQueue = new Queue<Message>();
        private void Send(ref Socket clientSocket)
        {
            while (clientSocket != null)
            {
                if (messageQueue.Count == 0) continue;
                var message = messageQueue.Dequeue();
                clientSocket.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message)));
            }
        }

        private void Receive(ref Socket clientSocket)
        {
            byte[] strbyte = new byte[1024];
            int count = clientSocket.Receive(strbyte);
            string ret = Encoding.UTF8.GetString(strbyte.SubArray(0, count));
            if (count > 0)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Num.Text = ret.Split(" ")[1];
                }));
            }

            while (true)
            {
                strbyte = new byte[1024];
                try
                {
                    count = clientSocket.Receive(strbyte);
                }
                catch
                {
                    break;
                }
                ret = Encoding.UTF8.GetString(strbyte.SubArray(0, count));
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

            this.Dispatcher.Invoke((Action)(() =>
            {
                Num.Text = "Null";
            }));
            clientSocket = null;
        }

        private void SwitchToSTA_ShowMessage(Message message)
        {
            General.roomStorage[targetAddress].Add(message);
            if (targetAddress != Room.currentAddress)
                return;

            this.Dispatcher.Invoke(() =>
            {
                Room.AddMessage(message);
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
