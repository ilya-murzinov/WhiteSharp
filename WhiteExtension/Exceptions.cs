using TestStack.White;

namespace WhiteExtension
{
    internal class WindowNotFoundException : WhiteException
    {
        public WindowNotFoundException(string message)
            : base(message)
        {
        }
    }

    internal class ControlNotFoundException : WhiteException
    {
        public ControlNotFoundException(string message)
            : base(message)
        {
        }
    }

    internal class Exceptions : WhiteException
    {
        public Exceptions(string message) : base(message)
        {
        }
    }

    internal class AssertException : WhiteException
    {
        public AssertException(string message)
            : base(message)
        {
        }
    }
}