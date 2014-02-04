using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestStack.White.WindowsAPI;

namespace WhiteSharp
{
    public enum Keys
    {
        F5, Tab, Esc, Enter, Down, Del, CtrlA, CtrlEnter
    }

    class Keyboard
    {
        private static readonly Dictionary<Keys, Action> KeysDistionary = new Dictionary
            <Keys, Action>();

        static Keyboard()
        {
            KeysDistionary.Add(Keys.F5, 
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F5));
            KeysDistionary.Add(Keys.Tab,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.TAB));
            KeysDistionary.Add(Keys.Esc,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.ESCAPE));
            KeysDistionary.Add(Keys.Enter,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN));
            KeysDistionary.Add(Keys.Del,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DELETE));
            KeysDistionary.Add(Keys.Down,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN));
            KeysDistionary.Add(Keys.CtrlA,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    TestStack.White.InputDevices.Keyboard.Instance.Enter("a");
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                });
            KeysDistionary.Add(Keys.CtrlEnter,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                });
        }

        public static void Send(Keys key)
        {
            KeysDistionary[key].Invoke();

            Logging.Sent(key.ToString("G"));
        }

        public static void Send(string value)
        {
            SendKeys.SendWait(value);

            Logging.Sent(value);
        }
    }
}