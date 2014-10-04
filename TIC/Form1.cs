using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tesseract;

namespace TIC
{
    public partial class TICForm : Form
    {
        public TICForm()
        {
            InitializeComponent();

            this.AcceptButton = GoButton;
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Clicking");
            InputBox.Clear();

            TesseractEngine engine = new TesseractEngine("tessdata", "eng");
            Page result = engine.Process(new Bitmap("text.png"), PageSegMode.AutoOnly);
            Console.Write(result.GetText());
            Console.WriteLine("done");

        }
    }
}
