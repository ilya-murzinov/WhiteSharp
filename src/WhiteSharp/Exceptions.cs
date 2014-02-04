using System;

namespace WhiteSharp
{
    [Serializable]
    public class GeneralException : Exception
    {
        public GeneralException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class WindowNotFoundException : GeneralException
    {
        public WindowNotFoundException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class ControlNotFoundException : GeneralException
    {
        public ControlNotFoundException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class ControlNotEnabledException : GeneralException
    {
        public ControlNotEnabledException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class AssertException : GeneralException
    {
        public AssertException(string message)
            : base(message)
        {
        }
    }
}