using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using TestStack.White.WindowsAPI;

namespace WhiteSharp
{
    public enum Keys
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Zero,
        F5,
        F9,
        Tab,
        Esc,
        Enter,
        Up,
        Down,
        Del,
        CtrlA,
        CtrlEnter,
        CtrlInsert,
        CtrlDel,
        Ctrl,
        AltF4,
        CtrlP,
        CtrlO,
        F2,
        Ins,
        F8,
        Space,
        F3,
        CtrlD,
        PageDown,
        PageUp,
        CtrlG
    }

    internal class Keyboard
    {
        private static Keyboard _instance;

        private readonly Dictionary<Keys, Action> _keysDistionary = new Dictionary
            <Keys, Action>();

        private Keyboard()
        {
            _keysDistionary.Add(Keys.One, () => SendKeys.SendWait("1"));
            _keysDistionary.Add(Keys.Two, () => SendKeys.SendWait("2"));
            _keysDistionary.Add(Keys.Three, () => SendKeys.SendWait("3"));
            _keysDistionary.Add(Keys.Four, () => SendKeys.SendWait("4"));
            _keysDistionary.Add(Keys.Five, () => SendKeys.SendWait("5"));
            _keysDistionary.Add(Keys.Six, () => SendKeys.SendWait("6"));
            _keysDistionary.Add(Keys.Seven, () => SendKeys.SendWait("7"));
            _keysDistionary.Add(Keys.Eight, () => SendKeys.SendWait("8"));
            _keysDistionary.Add(Keys.Nine, () => SendKeys.SendWait("9"));
            _keysDistionary.Add(Keys.Zero, () => SendKeys.SendWait("0"));
            _keysDistionary.Add(Keys.PageUp,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.PAGEUP));
            _keysDistionary.Add(Keys.PageDown,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.PAGEDOWN));
            _keysDistionary.Add(Keys.F2,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F2));
            _keysDistionary.Add(Keys.F3,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F3));
            _keysDistionary.Add(Keys.F5,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F5));
            _keysDistionary.Add(Keys.F8,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F8));
            _keysDistionary.Add(Keys.F9,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F9));
            _keysDistionary.Add(Keys.Tab,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.TAB));
            _keysDistionary.Add(Keys.Esc,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.ESCAPE));
            _keysDistionary.Add(Keys.Space,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.SPACE));
            _keysDistionary.Add(Keys.Enter,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN));
            _keysDistionary.Add(Keys.Del,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DELETE));
            _keysDistionary.Add(Keys.Down,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN));
            _keysDistionary.Add(Keys.Up,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.UP));
            _keysDistionary.Add(Keys.Ctrl,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.CONTROL));
            _keysDistionary.Add(Keys.Ins,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.INSERT));
            _keysDistionary.Add(Keys.CtrlA,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.Enter("a");
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    LeaveAllKeys();
                });
            _keysDistionary.Add(Keys.CtrlG,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.Enter("g");
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    LeaveAllKeys();
                });
            _keysDistionary.Add(Keys.CtrlD,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.Enter("d");
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    LeaveAllKeys();
                });
            _keysDistionary.Add(Keys.CtrlP,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.Enter("p");
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    LeaveAllKeys();
                });
            _keysDistionary.Add(Keys.CtrlO,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.Enter("o");
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    LeaveAllKeys();
                });
            _keysDistionary.Add(Keys.AltF4,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.ALT);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F4);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.ALT);
                    LeaveAllKeys();
                });
            _keysDistionary.Add(Keys.CtrlEnter,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    LeaveAllKeys();
                });
            _keysDistionary.Add(Keys.CtrlInsert,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.INSERT);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    LeaveAllKeys();
                });
            _keysDistionary.Add(Keys.CtrlDel,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DELETE);
                    Thread.Sleep(100);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    LeaveAllKeys();
                });
        }

        public Keys GetKeyByString(string str)
        {
            Keys result;
            switch (str)
            {
                case "0":
                    result = Keys.Zero;
                    break;
                case "1":
                    result = Keys.One;
                    break;
                case "2":
                    result = Keys.Two;
                    break;
                case "3":
                    result = Keys.Three;
                    break;
                case "4":
                    result = Keys.Four;
                    break;
                case "5":
                    result = Keys.Five;
                    break;
                case "6":
                    result = Keys.Six;
                    break;
                case "7":
                    result = Keys.Seven;
                    break;
                case "8":
                    result = Keys.Eight;
                    break;
                case "9":
                    result = Keys.Nine;
                    break;
                default:
                    result = Keys.Tab;
                    break;
            }
            return result;
        }

        public static Keyboard Instance
        {
            get { return _instance ?? (_instance = new Keyboard()); }
        }

        public void Send(Keys key)
        {
            _keysDistionary[key].Invoke();
            LeaveAllKeys();
        }

        public void LeaveAllKeys()
        {
            TestStack.White.InputDevices.Keyboard.Instance.LeaveAllKeys();
        }
    }
}