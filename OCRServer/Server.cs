using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace OCR
{
    class Server
    {
        private StreamSocketListener serverListener;
        public static String port = "3011";
        public static String adressIP = "192.168.173.1";
        private HostName hostName;

        //Initialize the server
        public Server()
        {
            serverListener = new StreamSocketListener();
            hostName = new HostName(adressIP);
            listen();
        }

        //Create the listener which is waiting for connection
        private async void listen()
        {
            serverListener.ConnectionReceived += OnConnection;
            try
            {

                //await serverListener.BindEndpointAsync(hostName, port);
                await serverListener.BindServiceNameAsync(port);
                //MainWindow.Current.UpdateLog("Listening for Connection(s)");
            }
            catch (Exception exception)
            {
                //MainWindow.Current.UpdateLog("Exception throw in Listen : " + exception);
            }

        }

        //When a connection appears, this function his called
        private async void OnConnection(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {

            //MainWindow.Current.UpdateLog("A message has been received...");
            //if (MainWindow.Current.loadingPage)
            //{
            //    MainWindow.Current.UpdateLog("wait please");
            //}


            DataReader reader = new DataReader(args.Socket.InputStream);

            try
            {
                byte[] inStream;
                reader.ReadBytes(out inStream);
                ImageConverter ic = new ImageConverter();
                Image img = (Image)ic.ConvertFrom(inStream);
                Bitmap bit = new Bitmap(img);
                var ocrResult = await ocrEngine.RecognizeAsync((uint)bitmap.PixelHeight, (uint)bitmap.PixelWidth, bitmap.PixelBuffer.ToArray());

                var lines = ocrResult.Lines;

                JsonArray jsonLines = new JsonArray();
                foreach(var line in lines) {
                    JsonArray jsonWords = new JsonArray();
                    foreach(var word in line.Words) {
                        JsonObject wordObject = new JsonObject();
                        String wordText = word.Text;
                        int x = word.Left + word.Width / 2;
                        int y = word.Top + word.Height / 2;
                        wordObject.Add("text", wordText);
                        wordObject.Add("x", x);
                        wordObject.Add("y", y);

                        jsonWords.add(wordObject);
                    }
                    jsonLines.Add(jsonWords);
                }


                fjdkal; fjksda;
                var outStream = args.Socket.OuputStream;

                outStream.WriteAsync(jsonLines.Stringify());

            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                //MainWindow.Current.UpdateLog("Read stream failed with error: " + exception.Message);
            }
            finally { 
                
            }

        }

    }
}