using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace WhiteSharp.Drawing
{
    internal class ScreenRectangle
    {
        //TODO: Think about making color configurable
        private readonly Color _color = Color.Red;
        private readonly Form _form = new Form();

        internal ScreenRectangle(Rect rectangle)
        {
            _form.FormBorderStyle = FormBorderStyle.None;
            _form.ShowInTaskbar = false;
            _form.TopMost = true;
            _form.Left = 0;
            _form.Top = 0;
            _form.Width = 1;
            _form.Height = 1;
            _form.BackColor = _color;
            _form.Opacity = 0.8;
            _form.Visible = false;

            //Set popup style
            int num1 = NativeMethods.GetWindowLong(_form.Handle, -20);
            NativeMethods.SetWindowLong(_form.Handle, -20, num1 | 0x80);

            //Set position
            NativeMethods.SetWindowPos(_form.Handle, new IntPtr(-1), Convert.ToInt32(rectangle.X),
                Convert.ToInt32(rectangle.Y),
                Convert.ToInt32(rectangle.Width), Convert.ToInt32(rectangle.Height), 0x10);
        }

        internal virtual void Show()
        {
            NativeMethods.ShowWindow(_form.Handle, 8);
        }

        internal virtual void Hide()
        {
            _form.Hide();
        }
    }

    internal class FrameRectangle
    {
        //Using 4 rectangles to display each border
        private readonly ScreenRectangle[] rectangles;
        private ScreenRectangle bottomBorder;
        private ScreenRectangle leftBorder;
        private ScreenRectangle rightBorder;
        private ScreenRectangle topBorder;
        private int width = 3;

        internal FrameRectangle(Rect boundingRectangle)
        {
            leftBorder =
                new ScreenRectangle(new Rect(boundingRectangle.X - width, boundingRectangle.Y - width, width,
                    boundingRectangle.Height + 2*width));
            topBorder =
                new ScreenRectangle(new Rect(boundingRectangle.X, boundingRectangle.Y - width, boundingRectangle.Width,
                    width));
            rightBorder =
                new ScreenRectangle(new Rect(boundingRectangle.X + boundingRectangle.Width, boundingRectangle.Y - width,
                    width, boundingRectangle.Height + 2*width));
            bottomBorder =
                new ScreenRectangle(new Rect(boundingRectangle.X, boundingRectangle.Y + boundingRectangle.Height,
                    boundingRectangle.Width, width));
            rectangles = new[] {leftBorder, topBorder, rightBorder, bottomBorder};
        }

        internal virtual void Highlight()
        {
            rectangles.ToList().ForEach(x => x.Show());
            Thread.Sleep(1000);
            rectangles.ToList().ForEach(x => x.Hide());
        }
    }
}