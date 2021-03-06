﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace WhiteSharp
{
    internal static class NativeMethods
    {
        private const UInt32 SwpNosize = 0x0001;
        private const UInt32 SwpNomove = 0x0002;
        private const UInt32 SwpShowwindow = 0x0040;
        private static readonly IntPtr HwndTop = new IntPtr(0);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
            uint uFlags);

        public static void OnTop(this Window window)
        {
            if (window.DisplayState == WindowVisualState.Minimized)
                window.DisplayState = WindowVisualState.Maximized;
            SetWindowPos(new IntPtr(window.AutomationElement.Current.NativeWindowHandle), HwndTop, 0, 0, 0, 0,
                SwpNomove | SwpNosize | SwpShowwindow);
        }
    }
}