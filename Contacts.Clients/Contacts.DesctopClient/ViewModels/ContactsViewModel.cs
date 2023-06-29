using System.Collections.ObjectModel;

namespace Contacts.DesctopClient.ViewModels;

public class ContactsViewModel
{
    public ObservableCollection<ContactViewModel> Contacts { get; set; } = new();
}
