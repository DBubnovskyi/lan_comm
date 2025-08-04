using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace MulticastListener
{
    class MulticastChat
    {
        static readonly string multicastIp = "239.0.0.222";
        static readonly int multicastPort = 12345;

        public static void Run()
        {
            Console.WriteLine("UDP Multicast Chat");
            Console.WriteLine($"Група: {multicastIp}:{multicastPort}");

            Thread listenerThread = new Thread(ListenMulticast);
            listenerThread.IsBackground = true;
            listenerThread.Start();

            using (UdpClient sender = new UdpClient())
            {
                sender.JoinMulticastGroup(IPAddress.Parse(multicastIp));
                sender.MulticastLoopback = false;

                IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(multicastIp), multicastPort);

                while (true)
                {
                    Console.Write("> ");
                    string message = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(message)) continue;

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    sender.Send(data, data.Length, groupEP);
                }
            }
        }

        static void ListenMulticast()
        {
            UdpClient receiver = new UdpClient();
            receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            receiver.Client.Bind(new IPEndPoint(IPAddress.Any, multicastPort));
            receiver.JoinMulticastGroup(IPAddress.Parse(multicastIp));

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, multicastPort);

            while (true)
            {
                byte[] data = receiver.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"\n[{remoteEP.Address}]: {message}");
                Console.Write("> ");
            }
        }
    }
}
