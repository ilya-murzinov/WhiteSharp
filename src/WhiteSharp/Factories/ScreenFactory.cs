using System;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using WhiteSharp.Attributes;

namespace WhiteSharp.Factories
{
    public class ScreenFactory
    {
        private static String _title;
        private static Window _window;
        private static By _by;
        private static int _index;

        public static void InitControls<T>(T screen, string title = "") where T : ScreenObject
        {
            var fields = screen.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance); 
            fields.ForEach(field =>
            {
                if (field.FieldType == typeof (Window))
                {
                    _title = title.Equals("") ? ((WindowAttribute) field.GetCustomAttributes(true).First()).Title : title;
                    _window = new Window(_title);
                    try
                    {
                        field.SetValue(screen, _window);
                    }
                    catch (Exception)
                    {
                    }
                }
            });

            if (_window == null && screen.GetType().BaseType != null)
            {
                fields = screen.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                fields.ForEach(field =>
                {
                    if (field.FieldType == typeof (Window))
                    {
                        _title = title.Equals("") ? ((WindowAttribute)field.GetCustomAttributes(true).First()).Title : title;
                        _window = new Window(_title);
                        try
                        {
                            field.SetValue(screen, _window);
                        }
                        catch (Exception)
                        {
                        }
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
                field.GetCustomAttributes(true).ForEach(attr =>
                {
                    if (attr is FindByAttribute)
                    {
                        var findsByAttribute = (FindByAttribute) attr;
                        if (findsByAttribute.Index != 0)
                        {
                            _index = findsByAttribute.Index;
                        }

                        if (_by.Result == null)
                        {
                            _by = findsByAttribute.Finder;
                        }
                        else
                        {
                            _by.Add(findsByAttribute.Finder);
                        }

                        field.SetValue(screen, _window.FindControl(_by, _index));
                    }
                });
            });
        }
    }
}