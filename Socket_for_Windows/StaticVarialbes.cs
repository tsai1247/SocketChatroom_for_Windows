using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Socket_for_Windows
{
    class User
    {
        public static string NickyName = "tsai1247";
    }

    class Network
    {
        public static string DestHost = "";
        public static string DestPort = "";

        public static string localHost = "";

        public static void refreshHost()
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localHost = endPoint.Address.ToString();
                    socket.Close();
                }
            }
            catch
            {
                localHost = "未連接網路";
                localHost = "192.168.56.1";
            }
        }
        public static List<Address> addressList = new List<Address>();

        internal static IPEndPoint ToIPEndPoint(Address address)
        {
            return new IPEndPoint(IPAddress.Parse(address.Host), int.Parse(address.Port));
        }
    }

    static class ExtensionFunctions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }

    public struct Address
    {
        public static Address GetRandomPort(List<Address> list = null)
        {
            if (list == null)
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

        internal bool IsValid()
        {
            try
            {
                int tmpPort = int.Parse(Port);
                if (tmpPort > 65535 || tmpPort < 1)
                    return false;
            }
            catch
            {
                return false;
            }

            var tmpHost = Host.Split(".");
            if (tmpHost.Length != 4) return false;
            foreach(var i in tmpHost)
            {
                try
                {
                    int tmpHostContent = int.Parse(i);
                }
                catch
                {
                    return false;
                }
            }
            return true;
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

        public Address(string host, string port) : this()
        {
            Host = host;
            Port = port;
        }
        public static bool operator ==(Address c1, Address c2)
        {
            return c1.Host == c2.Host && c1.Port == c2.Port;
        }
        public static bool operator !=(Address c1, Address c2)
        {
            return !(c1 == c2);
        }
    }

    public struct Message
    {
        public string nickyName;
        public string content;
        public DateTime dateTime;

        public Message(string nickyName, string content, DateTime dateTime) : this()
        {
            this.nickyName = nickyName;
            this.content = content;
            this.dateTime = dateTime;
        }
    }

    public class General
    {
        public static Dictionary<Address, List<Message>> roomStorage = new Dictionary<Address, List<Message>>();
        public static Dictionary<Address, OneClientRoomInfo> clientRoom = new Dictionary<Address, OneClientRoomInfo>();
        public static Dictionary<Address, OneRoomInfo> serverRoom = new Dictionary<Address, OneRoomInfo>();
        public static List<Socket> activeSocket = new List<Socket>();

        public static bool isClosing = false;
        public static MainWindow GetMainWindow()
        {
            return (Application.Current.MainWindow as MainWindow);
        }
    }
}
