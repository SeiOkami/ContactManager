using System.Collections;

namespace Contacts.WebClient.Models
{
    public class ContactsModel
    {
        public List<ContactModel> Contacts { get; set; } = new();
        public UserModel? User { get; set; }
        public bool IsThisUser { get; set; }
    }
}
