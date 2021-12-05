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
                server_address = STAGetAddress();
                
                General.clientRoom.Add(server_address, this);
                General.roomStorage.Add(server_address, new List<Message>());

                Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Address local_address = Address.GetRandomPort();
                server_socket.Bind(Network.ToIPEndPoint(local_address));
                General.activeSocket.Add(server_socket);
                while (true)
                {
                    try
                    {
                        server_socket.Connect(new IPEndPoint(IPAddress.Parse(server_address.Host), int.Parse(server_address.Port)));
                        break;
                    }
                    catch(SocketException e)
                    {
                        Thread.Sleep(1000);
                    }
                    catch(ObjectDisposedException e)
                    {
                        return;
                    }
                }
                string nickyName = "";
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (this == Room.currentRoom)
                    {
                        General.GetMainWindow().Room.chooseHint.Text = "";
                        General.GetMainWindow().Room.Send.IsEnabled = true;
                    }
                    nickyName = General.GetMainWindow().chatRoomList.nickyName.Text;
                }));
                server_socket.Send(Encoding.ASCII.GetBytes("Name " + nickyName));
                new Thread(() =>
                {
                    Send(ref server_socket);
                }).Start();

                new Thread(() =>
                {
                    Receive(ref server_socket);
                }).Start();
            }).Start();
        }

        public Queue<Message> messageQueue = new Queue<Message>();
        private void Send(ref Socket server_socket)
        {
            while (server_socket != null)
            {
                if (messageQueue.Count == 0) continue;
                var message = messageQueue.Dequeue();
                server_socket.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message)));
            }
        }

        private void Receive(ref Socket server_socket)
        {
            byte[] strbyte = new byte[1024];
            int count = server_socket.Receive(strbyte);
            string ret = Encoding.UTF8.GetString(strbyte.SubArray(0, count));
            if (count > 0)
            {
                string newMember = ret.Split(" ")[1];
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Num.Text = newMember;
                    if (member == null)
                        member = newMember;
                    else
                        member += "," + newMember;
                }));
                if (!General.members.ContainsKey(server_address))
                    General.members.Add(server_address, new List<string>());
                General.members[server_address].Add(newMember);
            }

            while (!General.isClosing)
            {
                strbyte = new byte[1024];
                try
                {
                    count = server_socket.Receive(strbyte);
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
                        SwitchToSTA_ShowMessage(new Message("error", "errorMessage", DateTime.Now));
                    }
                }
            }
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Num.Text = "Null";
                    member = null;
                }));
                server_socket = null;
            }
            catch { }

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

        public string member = null;
        private void enterRoom_Click(object sender, RoutedEventArgs e)
        {
            Room.ClearRoom();
            Room.RestoreRoom(this, server_address);

            if (member == null)
            {
                General.GetMainWindow().Room.chooseHint.Text = "等待連線中...";
                General.GetMainWindow().Room.Send.IsEnabled = false;
            }
            else
            {
                General.GetMainWindow().Room.chooseHint.Visibility = Visibility.Hidden;
                General.GetMainWindow().Room.Send.IsEnabled = true;
            }
        }
    }
}
