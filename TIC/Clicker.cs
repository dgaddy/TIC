using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIC
{
    public class Clicker
    {
        public int xCoordMouse;
        public int yCoordMouse;
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        
        public Clicker(){
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public static void clickSequence(Point? loc)
        {
            if (loc == null)
                return;
            Point clkPos = (Point) loc;
            Point curPos = Cursor.Position;
            //int returnXCoord = this.Location.X + this.Size.Width / 2 - 40;
            //int returnYCoord = InputBox.Location.Y + InputBox.Size.Height / 2;
            Clicker.toClick(clkPos.X, clkPos.Y, false);
            //Clicker.toClick(returnXCoord, returnYCoord);
            Clicker.toMove(curPos.X, curPos.Y);
        }

        public static void toClick(int xCoordMouse, int yCoordMouse, Boolean singleClick = true)
        {
            if (singleClick){
                SetCursorPos(xCoordMouse, yCoordMouse);
                mouse_event(MOUSEEVENTF_LEFTDOWN, xCoordMouse, yCoordMouse, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, xCoordMouse, yCoordMouse, 0, 0);
                //return position
            }
            else {
                SetCursorPos(xCoordMouse, yCoordMouse);
                mouse_event(MOUSEEVENTF_LEFTDOWN, xCoordMouse, yCoordMouse, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, xCoordMouse, yCoordMouse, 0, 0);
                System.Threading.Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_LEFTDOWN, xCoordMouse, yCoordMouse, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, xCoordMouse, yCoordMouse, 0, 0);
            }
        }
        public static void toMove(int xCoordMouse, int yCoordMouse)
        {
            SetCursorPos(xCoordMouse, yCoordMouse);
        }

    }
}
