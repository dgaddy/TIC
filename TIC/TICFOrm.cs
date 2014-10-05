using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GlobalHotkeys;
using System.Drawing.Imaging;

namespace TIC
{
    public partial class TICForm : Form
    {
        GlobalHotkey hotkey_;
        ScreenShotCreator screenShotCreator_ = new ScreenShotCreator();
        JsonResponse jsonResponse = new JsonResponse();
        OcrSocketWrapper ocr = OcrSocketWrapper.Instance;

        public TICForm()
        {
            InitializeComponent();

            this.TopMost = true;
            this.Visible = true;

            this.BackColor = Color.Magenta;
            this.TransparencyKey = this.BackColor;
            this.FormBorderStyle = FormBorderStyle.None;
            
            this.AcceptButton = GoButton;

            this.Top = 0;
            hotkey_ = new GlobalHotkey(Modifiers.Ctrl, Keys.Home, this, true);
            this.Hide();
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            string searchText = this.InputBox.Text;
            this.InputBox.Clear();
            this.Hide();
            
            String json = ocr.GetJson();
            if (json == null) {
                Console.WriteLine("error");
                return;
            }
            JsonWord[][] response = jsonResponse.ParseJson(json);
            Point loc = jsonResponse.getLocFromWord(searchText, response);


            int mousePositionX = MousePosition.X;
            int mousePositionY = MousePosition.Y;
            int returnXCoord = this.Location.X + this.Size.Width/2-40;
            int returnYCoord = InputBox.Location.Y + InputBox.Size.Height/2;
            Clicker.toClick(loc.X, loc.Y, true);
            //Clicker.toClick(returnXCoord, returnYCoord);
            //Clicker.toMove(mousePositionX, mousePositionY);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == GlobalHotkeys.Win32.WM_HOTKEY_MSG_ID)
            {
                Bitmap screenShot = screenShotCreator_.CreateDesktopScreenShot();
                ocr.SendBitmap(screenShot);

                this.Visible = true;
                this.Activate();
            }
            base.WndProc(ref m);
        }
    }
}
