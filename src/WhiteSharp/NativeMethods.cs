using System;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace WhiteSharp
{
    static class NativeMethods
    {
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            uint uFlags);

        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 SWP_SHOWWINDOW = 0x0040;

        public static void OnTop(this Window window)
        {
            if (window.DisplayState == WindowVisualState.Minimized)
                window.DisplayState = WindowVisualState.Maximized;

            SetWindowPos(new IntPtr(window.AutomationElement.Current.NativeWindowHandle), HWND_TOP, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
        }
    }
}
