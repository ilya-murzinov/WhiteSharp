using System;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace WhiteSharp
{
    static class NativeMethods
    {
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
            uint uFlags);

        private static readonly IntPtr HwndTop = new IntPtr(0);

        private const UInt32 SwpNosize = 0x0001;
        private const UInt32 SwpNomove = 0x0002;
        private const UInt32 SwpShowwindow = 0x0040;

        public static void OnTop(this Window window)
        {
            if (window.DisplayState == WindowVisualState.Minimized)
                window.DisplayState = WindowVisualState.Maximized;
            SetWindowPos(new IntPtr(window.AutomationElement.Current.NativeWindowHandle), HwndTop, 0, 0, 0, 0,
                SwpNomove | SwpNosize | SwpShowwindow);
        }
    }
}