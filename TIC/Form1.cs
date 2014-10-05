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

using Tesseract;

namespace TIC
{
    public partial class TICForm : Form
    {
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
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            //InputBox.Clear();

            //TesseractEngine engine = new TesseractEngine("tessdata", "eng");
            int mousePositionX = MousePosition.X;
            int mousePositionY = MousePosition.Y;
            int returnXCoord = this.Location.X + this.Size.Width/2-40;
            int returnYCoord = InputBox.Location.Y + InputBox.Size.Height/2;
            Clicker.toClick(1500, 400,false);
            Clicker.toClick(returnXCoord, returnYCoord);
            Clicker.toMove(mousePositionX, mousePositionY);
        }
    }
}
