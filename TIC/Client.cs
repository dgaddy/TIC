using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.IO;
using System.Threading;

namespace TIC
{
    class ClientSocket
    {
        Socket socket_;
        private const string SERVER_ADDRESS = "ec2-54-69-211-220.us-west-2.compute.amazonaws.com ";
        private const int SERVER_PORT = 3011;

        public ClientSocket() 
        { 
            startListening();
        }

        private void startListening()
        {
            socket_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPHostEntry ipHostInfo = Dns.GetHostEntry(SERVER_ADDRESS);
            IPEndPoint ipEp = new IPEndPoint(ipHostInfo.AddressList[0], SERVER_PORT);

            socket_ = new Socket(AddressFamily.InterNetwork,
                                 SocketType.Stream, ProtocolType.Tcp);

            socket_.Connect(ipEp);
        }

        private void stopListenting()
        {
            socket_.Shutdown(SocketShutdown.Both);
            socket_.Close();
        }

        /// <summary>
        /// //////////////////////////
        /// </summary>
        public void SendData(byte[] data)
        {
            socket_.Send(data);
        }

        public void ReceiveData(byte[] data)
        {
            int nBytes = socket_.Available;
            if (nBytes > 0)
            {
                data = new byte[nBytes];
                socket_.Receive(data);
            }
            else
            {
                data = null;
            }
        }


        ~ClientSocket()
        {
            stopListenting();
        }
    }
}
