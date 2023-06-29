namespace Contacts.Shared.Models;

public class ContactsModel
{
    public List<ContactModel> Contacts { get; set; } = new();
    public UserInfoModel? User { get; set; }
    public bool IsThisUser { get; set; }
}