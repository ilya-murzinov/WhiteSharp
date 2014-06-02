using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestStack.White.WindowsAPI;

namespace WhiteSharp
{
    public enum Keys
    {
        F5,
        Tab,
        Esc,
        Enter,
        Down,
        Del,
        CtrlA,
        CtrlEnter
    }

    internal class Keyboard
    {
        private static Keyboard _instance;

        private readonly Dictionary<Keys, Action> _keysDistionary = new Dictionary
            <Keys, Action>();

        private Keyboard()
        {
            _keysDistionary.Add(Keys.F5,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.F5));
            _keysDistionary.Add(Keys.Tab,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.TAB));
            _keysDistionary.Add(Keys.Esc,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.ESCAPE));
            _keysDistionary.Add(Keys.Enter,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN));
            _keysDistionary.Add(Keys.Del,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DELETE));
            _keysDistionary.Add(Keys.Down,
                () => TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN));
            _keysDistionary.Add(Keys.CtrlA,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    TestStack.White.InputDevices.Keyboard.Instance.Enter("a");
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                });
            _keysDistionary.Add(Keys.CtrlEnter,
                () =>
                {
                    TestStack.White.InputDevices.Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    TestStack.White.InputDevices.Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                    TestStack.White.InputDevices.Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                });
        }

        public static Keyboard Instance
        {
            get { return _instance ?? (_instance = new Keyboard()); }
        }

        public void Send(Keys key)
        {
            _keysDistionary[key].Invoke();
        }

        public void Send(string value)
        {
            SendKeys.SendWait(value);
        }
    }
}