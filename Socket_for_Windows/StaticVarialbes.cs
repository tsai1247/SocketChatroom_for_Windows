using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
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
        public static Dictionary<Address, List<string>> members = new Dictionary<Address, List<string>>();
        public static List<Socket> activeSocket = new List<Socket>();
        public static List<OneClientRoomInfo> allClientRooms = new List<OneClientRoomInfo>();
        public static List<OneRoomInfo> allServerRooms = new List<OneRoomInfo>();

        public static bool isClosing = false;
        public static MainWindow GetMainWindow()
        {
            return (Application.Current.MainWindow as MainWindow);
        }
    }

    public class MyColor
    {
        public static Color roomInfoColor;
    }


    public class Transfer
    {
        public static Color Brush2Color(Brush brush)
        {
            return ((SolidColorBrush)brush).Color;
        }

        public static Brush Color2Brush(Color color)
        {
            return new SolidColorBrush(color);
        }
        public static Brush Color2Brush(Color? color)
        {
            return new SolidColorBrush((Color)color);
        }

        public static Color String2Color(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        public static Color String2Color(byte[] b)
        {
            return String2Color(Byte2Str(b));
        }

        public static Color String2Color(object b)
        {
            return String2Color((byte[]) b);
        }

        public static string Byte2Str(byte[] b)
        {
            return Encoding.UTF8.GetString(b);
        }
    }
}
