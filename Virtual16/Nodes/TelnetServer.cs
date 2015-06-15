using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Cyperus;

namespace Virtual16.Nodes
{
    public class TelnetCommand
    {
        public readonly string Value;
        public readonly NetworkStream Stream;

        public TelnetCommand(string val, NetworkStream stream)
        {
            Value = val;
            Stream = stream;
        }
    }

    public class TelnetClient
    {
        public readonly NetworkStream Stream;

        public TelnetClient(NetworkStream stream)
        {
            Stream = stream;
        }
    }
    
    public class TelnetServer : Producer
    {
        protected readonly TcpListener Listener;
        protected readonly Socket<TelnetCommand> Out;
        protected readonly Socket<TelnetClient> ConnectionOutput;

        private readonly string Greeting = "  Virtual16 CPU emulator powered by Cyperus\r\n  Written by icefairy64, 2015\r\n\r\n";

        public TelnetServer(string name, Cyperus.Environment env)
            : base(name, env)
        {
            Listener = new TcpListener(IPAddress.Any, 23);
            Out = AddOutput<TelnetCommand>("out");
            ConnectionOutput = AddOutput<TelnetClient>("onConnect");
        }

        public static string ByteArrayToString(byte[] array, int len)
        {
            var query =
                from x in array.Take(len)
                select (char)x;

            return new String(query.ToArray());
        }

        public static byte[] StringToByteArray(string str)
        {
            var query =
                from x in str.ToCharArray()
                select (byte)x;

            return query.ToArray();
        }

        protected override void Produce()
        {
            Listener.Start();
            try
            {
                while (true)
                {
                    try
                    {
                        while (FPaused || !Listener.Pending())
                            Thread.Sleep(1000);

                        var client = Listener.AcceptTcpClient();
                        var stream = client.GetStream();

                        SendToSocket(ConnectionOutput, new TelnetClient(stream));

                        stream.Write(StringToByteArray(Greeting), 0, Greeting.Length);
                        stream.Write(StringToByteArray("  > "), 0, 4);

                        var buf = new byte[1024];
                        int i = 0;
                        while ((i = stream.Read(buf, 0, buf.Length)) != 0)
                        {
                            var str = ByteArrayToString(buf, i);

                            if (str[0] == 0xff)
                                continue;

                            if (str == "\r\n")
                                stream.Write(StringToByteArray("  > "), 0, 4);
                            else
                            {
                                // Dispatching received command
                                SendToSocket(Out, new TelnetCommand(str, stream)).Wait();

                                if (str.Contains("exit"))
                                {
                                    stream.Write(new byte[1] { (byte)'9' }, 0, 1);
                                    stream.Close();
                                    client.Close();
                                    break;
                                }
                            }
                        }
                    }
                    catch (ThreadInterruptedException e)
                    {
                        continue;
                    }
                }
            }
            catch (SocketException e)
            {
                return;
            }
            catch (ThreadAbortException e)
            {
                Listener.Stop();
                return;
            }
        }
    }
}
