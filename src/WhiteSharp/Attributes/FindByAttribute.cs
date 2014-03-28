using System;

namespace WhiteSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class FindsByAttribute : Attribute
    {
        private By _finder;

        public How How { get; set; }

        public string Using { get; set; }

        internal By Finder
        {
            get { return _finder = By.Create(How, Using); }
            set { _finder = value; }
        }
    }
}