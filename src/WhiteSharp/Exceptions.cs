using System;
using TestStack.White;

namespace WhiteSharp
{
    [Serializable]
    public class GeneralException : WhiteException
    {
        public GeneralException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class WindowNotFoundException : WhiteException
    {
        public WindowNotFoundException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class ControlNotFoundException : WhiteException
    {
        public ControlNotFoundException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class ControlNotEnabledException : WhiteException
    {
        public ControlNotEnabledException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class AssertException : WhiteException
    {
        public AssertException(string message)
            : base(message)
        {
        }
    }
}