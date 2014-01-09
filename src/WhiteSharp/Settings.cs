using System.ComponentModel;
using System.Configuration;

namespace WhiteSharp
{
    // Этот класс позволяет обрабатывать определенные события в классе параметров:
    //  Событие SettingChanging возникает перед изменением значения параметра.
    //  Событие PropertyChanged возникает после изменения значения параметра.
    //  Событие SettingsLoaded возникает после загрузки значений параметров.
    //  Событие SettingsSaving возникает перед сохранением значений параметров.
    public sealed partial class Settings
    {
        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
            // Добавьте здесь код для обработки события SettingChangingEvent.
        }

        private void PropertyChangingEventHandler(object sender, PropertyChangedEventArgs e)
        {
            new Strings();
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
            // Добавьте здесь код для обработки события SettingsSaving.
        }
    }
}