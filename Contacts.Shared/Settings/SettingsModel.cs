namespace Contacts.Shared.Settings;

public class SettingsModel
{
    public SettingsModelApi WebAPI { get; set; } = null!;
    public SettingsModelIdentity Identity { get; set; } = null!;
}
