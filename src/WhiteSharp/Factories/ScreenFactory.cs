﻿using System;
using System.Reflection;
using Castle.Core.Internal;
using WhiteSharp.Attributes;

namespace WhiteSharp.Factories
{
    public class ScreenFactory
    {
        private static Window _window;
        private static By _by = new By();

        public static void InitControls(object screen)
        {
            FieldInfo[] fields = screen.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            fields.ForEach(field =>
            {
                if (field.FieldType == typeof (Window))
                {
                    _window = (Window) field.GetValue(screen);
                }
            });

            if (_window == null)
            {
                throw new Exception("Window was not found in the screen object.");
            }

            fields.ForEach(field =>
            {
                field.GetCustomAttributes().ForEach(attr =>
                {
                    if (attr is FindsByAttribute)
                    {
                        FindsByAttribute findsByAttribute = (FindsByAttribute) attr;
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
