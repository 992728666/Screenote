using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Screenote
{
    static class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, Keys key);

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Screen screen = new Screen();
            RegisterHotKey(screen.Handle, 936, 0, Keys.PrintScreen);
            RegisterHotKey(screen.Handle, 936, 0, Keys.Pause);
            screen.Opacity = 0;
            screen.Show();
            Application.Run();
        }
    }
}
