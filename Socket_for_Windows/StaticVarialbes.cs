﻿using System;
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
        public static MainWindow GetMainWindow()
        {
            return (Application.Current.MainWindow as MainWindow);
        }
    }
}
