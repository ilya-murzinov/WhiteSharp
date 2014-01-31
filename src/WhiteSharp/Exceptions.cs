using TestStack.White;

namespace WhiteSharp
{
    public class GeneralException : WhiteException
    {
        public GeneralException(string message)
            : base(message)
        {
        }
    }

    public class WindowNotFoundException : WhiteException
    {
        public WindowNotFoundException(string message)
            : base(message)
        {
        }
    }

    public class ControlNotFoundException : WhiteException
    {
        public ControlNotFoundException(string message)
            : base(message)
        {
        }
    }

    public class ControlNotEnabledException : WhiteException
    {
        public ControlNotEnabledException(string message)
            : base(message)
        {
        }
    }

    public class AssertException : WhiteException
    {
        public AssertException(string message)
            : base(message)
        {
        }
    }
}