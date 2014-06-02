using System;

namespace WhiteSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class WindowAttribute : Attribute
    {
        public WindowAttribute(String title)
        {
            Title = title;
        }

        public String Title { get; set; }
    }
}