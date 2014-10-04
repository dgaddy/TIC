using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }
    }
}
