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

                int height = bmp.Height;
                int width = bmp.Width;
                //// Send Width and Height
                //byte[] width = BitConverter.GetBytes(bmp.Width);
                //socket_.SendData(width);
                //byte[] height = BitConverter.GetBytes(bmp.Height);
                //socket_.SendData(height);
                //byte[] size = width.Concat(height).ToArray();
                //socket_.SendData(size);



            // Lock the bitmap's bits.  
            /*Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes  = Math.Abs(bmpData.Stride) * bmp.Height;
             byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.   
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
                rgbValues[counter] = 255;

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);*/
            
                //bgra
                int loc = 0;
                byte[] data = new byte[width * height * 4];
                for(int i = 0; i < height; i++) {
                    for(int j = 0; j < width; j++) {
                        Color color = bmp.GetPixel(j, i);
                        data[loc] = color.B;
                        loc++;
                        data[loc] = color.G;
                        loc++;
                        data[loc] = color.R;
                        loc++;
                        data[loc] = color.A;
                        loc++;
                    }
                }
            

                byte[] conc = BitConverter.GetBytes(bmp.Width).Concat(BitConverter.GetBytes(bmp.Height)).Concat(BitConverter.GetBytes(data.Length)).Concat(data).ToArray();
                
                socket_.SendData(conc);
                //socket_.SendData((BitConverter.GetBytes(bmp.Height)));
                //socket_.SendData((BitConverter.GetBytes(bmpBytes.Length)));
                for (int i = 0; i < 15; i++) {
                    Console.Write(conc[i]+",");
                }
                    
                Console.WriteLine(bmp.Width);
                Console.WriteLine(bmp.Height);
                Console.WriteLine(data.Length);
                //socket_.SendData(bmpBytes);

                
            }
        }

        public string GetJson()
        {
            byte[] data = socket_.ReceiveData();
            if (data != null)
            {
                return Encoding.UTF8.GetString(data);
            }
            return null;
        }
    }
}
