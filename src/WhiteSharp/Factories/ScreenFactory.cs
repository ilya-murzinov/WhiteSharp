using System;
using System.Reflection;
using Castle.Core.Internal;
using WhiteSharp.Attributes;

namespace WhiteSharp.Factories
{
    public class ScreenFactory
    {
        private static Window _window;
        private static By _by;

        public static void InitControls<T>(T screen) where T : ScreenObject
        {
            FieldInfo[] fields = screen.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            fields.ForEach(field =>
            {
                if (field.FieldType == typeof (Window))
                {
                    _window = (Window) field.GetValue(screen);
                }
            });

            if (_window == null && screen.GetType().BaseType != null)
            {
                fields = screen.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                fields.ForEach(field =>
                {
                    if (field.FieldType == typeof(Window))
                    {
                        _window = (Window)field.GetValue(screen);
                    }
                });
            }

            if (_window == null)
            {
                throw new Exception("Window was not found in the screen object.");
            }

            fields.ForEach(field =>
            {
                _by = new By();
                field.GetCustomAttributes().ForEach(attr =>
                {
                    if (attr is FindsByAttribute)
                    {
                        var findsByAttribute = (FindsByAttribute) attr;
                        if (_by.Result == null)
                        {
                            _by = findsByAttribute.Finder;
                        }
                        else
                        {
                            _by.Add(findsByAttribute.Finder);
                        }
                        field.SetValue(screen, _window.FindControl(_by));
                    }
                });
            });
        }
    }
}