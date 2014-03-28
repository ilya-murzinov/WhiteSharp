using System;
using System.Windows.Automation;

namespace WhiteSharp.Extensions
{
    internal static class ControlTypeExtensions
    {
        internal static ControlType FromString(string from)
        {
            ControlType type = ControlType.Window;
            try
            {
                if (from.Contains("."))
                {
                    return (ControlType) type.GetType()
                        .GetField(from.Substring(@from.IndexOf(".", StringComparison.Ordinal) + 1))
                        .GetValue(typeof (ControlType));
                }
                return (ControlType) type.GetType().GetField(from).GetValue(typeof (ControlType));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}