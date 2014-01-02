namespace WhiteExtension
{
    public class Config
    {
        public enum Languages
        {
            En = 0,
            Ru = 1
        }

        private static int _language = (int)Languages.En;

        public static int Language 
        {
            get 
            {
                return _language;
            }
            set
            {
                _language = value;
            } 
        }

        public static void SetLanguage(int language)
        {
            Language = language;
            new Strings();
        }

        public enum OutputLevel
        {
            Trace = 1,
            Standart = 2,
            File = 3
        }

        public static int Output = (int)OutputLevel.Standart;

        private const int DefaultTimeout = 30000;
        private const int DefaultDelay = 200;
        private static int _delay = DefaultDelay;
        private static int _timeout = DefaultTimeout;

        public static int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public static int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        public static void ResetTimeout()
        {
            Timeout = DefaultTimeout;
        }

        public static void ResetDelay()
        {
            Delay = DefaultDelay;
        }
    }
}