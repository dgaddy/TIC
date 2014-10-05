using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIC
{
    public partial class TICNumberOverlay : Form
    {
        List<Point> matchPoints_;
        Action<Point?> pointSelectedCallback_;
        bool drawn_ = false;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public TICNumberOverlay(List<Point> matchPoints, Action<Point?> pointSelectedCallback)
        {
            _proc = this.HookCallback;
            _hookID = SetHook(_proc);
            new TICOverlay();
            pointSelectedCallback_ = pointSelectedCallback;
            matchPoints_ = matchPoints;

            //InitializeComponent();
            this.TopMost = true;
            this.Visible = true;

            this.ShowInTaskbar = false;

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Blue;
            this.TransparencyKey = this.BackColor;
        }
        ~TICNumberOverlay()
        {
            UnhookWindowsHookEx(_hookID);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!drawn_)
            {
                Graphics gfx = e.Graphics;
                for (int i = 0; i < matchPoints_.Count; ++i)
                {
                    const int WIDTH = 30;
                    Label label = new Label();
                    label.Text = i.ToString();
                    label.ForeColor = Color.WhiteSmoke;
                    label.Font = new Font("Arial", 18);
                    label.Location = matchPoints_[i];
                    label.Size = new Size(WIDTH, WIDTH);
                    label.Show();
                    gfx.DrawEllipse(System.Drawing.Pens.Black, matchPoints_[i].X - WIDTH / 2, matchPoints_[i].Y / 2, WIDTH, WIDTH);
                    this.Controls.Add(label);
                }
                drawn_ = true;
            }
            base.OnPaint(e);
        }

        private void callCallback(Point? point)
        {
            this.Close();
            pointSelectedCallback_(point);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode >= 0x30 && vkCode <= 0x39)
                {
                    if (vkCode - 0x30 < matchPoints_.Count)
                    {
                        callCallback(matchPoints_[vkCode - 0x30]);
                    }
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
