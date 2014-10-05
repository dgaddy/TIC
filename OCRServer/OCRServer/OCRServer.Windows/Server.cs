using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

using Windows.Storage.Streams;
using Windows.Data.Json;

using WindowsPreview.Media.Ocr;

using Windows.UI.Xaml.Media.Imaging;

namespace OCRServer
{
    class Server
    {
        private StreamSocketListener serverListener;
        public static String port = "3011";
        

        private OcrEngine ocrEngine;

        //Initialize the server
        public Server()
        {
            ocrEngine = new OcrEngine(OcrLanguage.English);

            serverListener = new StreamSocketListener();
            
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
                
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                byte[] inStream = new byte[width*height];
                reader.ReadBytes(inStream);
                
                var ocrResult = await ocrEngine.RecognizeAsync((uint)width, (uint)height, inStream);

                var lines = ocrResult.Lines;

                JsonArray jsonLines = new JsonArray();
                foreach(var line in lines) {
                    JsonArray jsonWords = new JsonArray();
                    foreach(var word in line.Words) {
                        JsonObject wordObject = new JsonObject();
                        String wordText = word.Text;
                        int x = word.Left + word.Width / 2;
                        int y = word.Top + word.Height / 2;
                        
                        wordObject.Add("text", JsonValue.CreateStringValue(wordText));
                        wordObject.Add("x", JsonValue.CreateNumberValue(x));
                        wordObject.Add("y", JsonValue.CreateNumberValue(y));

                        jsonWords.Add(wordObject);
                    }
                    jsonLines.Add(jsonWords);
                }

                var outStream = args.Socket.OutputStream;

                var dataWriter = new DataWriter(args.Socket.OutputStream);
                dataWriter.WriteString(jsonLines.Stringify());

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