using Microsoft.Extensions.Configuration;

namespace Contacts.Shared.Settings;

public static class SettingsManager
{
    private static SettingsModel settings;
    public static SettingsModel Settings { get => settings; }
    static SettingsManager()
    {
        settings = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("Settings.json").Build().Get<SettingsModel>();
    }
}
