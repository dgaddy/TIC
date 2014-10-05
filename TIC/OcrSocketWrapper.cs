using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIC
{
    class OcrSocketWrapper
    {
        private ClientSocket socket_;
        private static OcrSocketWrapper instance_ = new OcrSocketWrapper();
        private OcrSocketWrapper()
        {
            socket_ = new ClientSocket();
        }

        public static OcrSocketWrapper Instance
        {
            get
            {
                return instance_;
            }
        }

        public void SendBitmap(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //// Send Width and Height
                //byte[] width = BitConverter.GetBytes(bmp.Width);
                //byte[] height = BitConverter.GetBytes(bmp.Height);
                //byte[] size = width.Concat(height).ToArray();
                //socket_.SendData(size);

                // Save to memory using the Jpeg format
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                // read to end
                byte[] bmpBytes = ms.GetBuffer();
                socket_.SendData(BitConverter.GetBytes(bmpBytes.Length));
                socket_.SendData(bmpBytes);
            }
        }

        public string GetJson()
        {
            byte[] data = null;
            socket_.ReceiveData(data);
            if (data != null)
            {
                return Encoding.UTF8.GetString(data);
            }
            return null;
        }
    }
}
