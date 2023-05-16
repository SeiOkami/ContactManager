namespace Contacts.WebClient.Services
{
    public class WebAPIServiceSettings
    {
        public string MainURL { get; set; } = String.Empty;

        public string ListContactsMethodName { get; set; } = String.Empty;
        public string DetailsContactMethodName { get; set; } = String.Empty;
        public string CreateContactMethodName { get; set; } = String.Empty;
        public string UpdateContactMethodName { get; set; } = String.Empty;
        public string DeleteContactMethodName { get; set; } = String.Empty;
        public string ClearContactsMethodName { get; set; } = String.Empty;
        public string GenerateContactsMethodName { get; set; } = String.Empty;
        public string ImportContactsMethodName { get; set; } = String.Empty;
        public string ListUsersMethodName { get; set; } = String.Empty;
        public string GetUserMethodName { get; set; } = String.Empty;

        public string ListContactsMethodURL { get => MainURL + ListContactsMethodName; }
        public string DetailsContactMethodURL { get => MainURL + DetailsContactMethodName; }
        public string CreateContactMethodURL { get => MainURL + CreateContactMethodName; }
        public string UpdateContactMethodURL { get => MainURL + UpdateContactMethodName; }
        public string DeleteContactMethodURL { get => MainURL + DeleteContactMethodName; }
        public string ClearContactsMethodURL { get => MainURL + ClearContactsMethodName; }
        public string GenerateContactsMethodURL { get => MainURL + GenerateContactsMethodName; }
        public string ImportContactsMethodURL { get => MainURL + ImportContactsMethodName; }
        public string ListUsersMethodURL { get => MainURL + ListUsersMethodName; }
        public string GetUserMethodURL { get => MainURL + GetUserMethodName; }

    }
}
