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
            InputBox.Clear();

            TesseractEngine engine = new TesseractEngine("tessdata", "eng");
            Page result = engine.Process(new Bitmap("text.png"), PageSegMode.AutoOnly);
            Console.Write(result.GetText());
            Console.WriteLine("done");

        }
    }
}
