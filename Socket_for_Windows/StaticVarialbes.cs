using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Socket_for_Windows
{
    class User
    {
        public static string NickyName = "tsai1247";
    }

    class Network
    {
        public static string localHost = "";

        public static void refreshHost()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localHost = endPoint.Address.ToString();
                socket.Close();
            }
        }
        public static List<Address> addressList = new List<Address>();

    }

    public struct Address
    {
        public static Address GetRandomPort(List<Address> list = null)
        {
            if(list == null)
            {
                list = Network.addressList;
            }

            Address ret = new Address();
            ret.Host = Network.localHost;

            while (true)
            {
                ret.Port = new Random().Next(1247, 2494).ToString();
                if (!list.Contains(ret))
                    break;
            }
            return ret;
        }

        public string Host;
        public string IP
        {
            get { return Host; }
            set
            {
                Host = value;
            }
        }
        public string Port;
    }
}
