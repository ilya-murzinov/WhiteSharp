using System;
using System.ComponentModel;

namespace WhiteSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class FindsByAttribute : Attribute
    {
        private By _finder;

        [DefaultValue(How.AutomationId)]
        public How How { get; set; }

        public string Using { get; set; }

        [DefaultValue(0)]
        public int Index { get; set; }

        public FindsByAttribute(string id)
        {
            Using = id;
        }

        public FindsByAttribute()
        {
        }

        internal By Finder
        {
            get { return _finder = By.Create(How, Using); }
            set { _finder = value; }
        }
    }
}