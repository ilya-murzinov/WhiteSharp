using TestStack.White;

namespace WhiteSharp
{
    internal class GeneralException : WhiteException
    {
        public GeneralException(string message)
            : base(message)
        {
        }
    }

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

    internal class ControlNotEnabledException : WhiteException
    {
        public ControlNotEnabledException(string message)
            : base(message)
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