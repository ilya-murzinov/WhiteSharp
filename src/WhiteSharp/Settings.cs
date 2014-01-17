using System.ComponentModel;
using System.Configuration;

namespace WhiteSharp
{
    public sealed partial class Settings
    {
        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
        }

        private void PropertyChangingEventHandler(object sender, PropertyChangedEventArgs e)
        {
            new Logging();
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
        }
    }
}