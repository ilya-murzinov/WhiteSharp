namespace WhiteSharp
{
    public abstract class ScreenObject
    {
        public void Log(string message)
        {
            Logging.TestStep(message);
        }
    }
}