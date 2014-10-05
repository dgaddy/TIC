using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.IO;

namespace TIC
{
    class Client
    {
        Socket server;
        private static Object clientLockObject_ = new Object();
        private static Client instance_;

        private Client() 
        { 
            server = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);
            startListening();
        }

        public static Client Instance
        {
            get
            {
                lock (clientLockObject_)
                {
                    if (instance_ == null)
                    {
                        instance_ = new Client();
                    }
                }
                return instance_;
            }
        }


        private void startListening()
        {

            Console.WriteLine("Server is starting...");
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);

            Socket newsock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);

            newsock.Bind(ipep);
            newsock.Listen(10);
            Console.WriteLine("Waiting for a client...");

            Socket client = newsock.Accept();
            IPEndPoint newclient = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with {0} at port {1}",
                            newclient.Address, newclient.Port);

            while (true)
            {
                data = ReceiveVarData(client);
                MemoryStream ms = new MemoryStream(data);
                try
                {
                    Image bmp = Image.FromStream(ms);
                    //pictureBox1.Image = bmp;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("something broke");
                }


                if (data.Length == 0)
                    newsock.Listen(10);
            }
            //Console.WriteLine("Disconnected from {0}", newclient.Address);
            client.Close();
            newsock.Close();
            /////////////////////////////////////////////

        }

        private static byte[] ReceiveVarData(Socket s)
        {
            int total = 0;
            int recv;
            byte[] datasize = new byte[4];

            recv = s.Receive(datasize, 0, 4, 0);
            int size = BitConverter.ToInt32(datasize, 0);
            int dataleft = size;
            byte[] data = new byte[size];


            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }

        public void SendBitmap(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Save to memory using the Jpeg format
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                // read to end
                byte[] bmpBytes = ms.GetBuffer();

                SendData(bmpBytes);
            }
        }

        /// <summary>
        /// //////////////////////////
        /// </summary>
        public void SendData(byte[] data)
        {
            int sent;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

            try
            {
                server.Connect(ipep);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server.");
                Console.WriteLine(e.ToString());
            }

            sent = SendVarData(server, data);

            Console.WriteLine("Disconnecting from server...");
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        private int SendVarData(Socket s, byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;

            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            sent = s.Send(datasize);

            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }
            return total;
        }
    }
}
