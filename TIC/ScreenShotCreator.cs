using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

public class ScreenShotCreator
{
    public Bitmap CreateDesktopScreenShot()
    {
        //Create a new bitmap.
        Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                          Screen.PrimaryScreen.Bounds.Height,
                                          PixelFormat.Format32bppArgb);

        // Create a graphics object from the bitmap.
        Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);

        // Take the screenshot from the upper left corner to the right bottom corner.
        gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0,
                                     0,
                                     Screen.PrimaryScreen.Bounds.Size,
                                     CopyPixelOperation.SourceCopy);

        return bmpScreenshot;
    }
}
