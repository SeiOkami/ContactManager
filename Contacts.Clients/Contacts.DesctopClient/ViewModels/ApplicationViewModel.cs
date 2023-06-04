using Contacts.DesctopClient.Identity;
using Contacts.DesctopClient.Models;
using Contacts.Shared.LaunchManager;
using IdentityModel.OidcClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Contacts.DesctopClient.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ContactModel> contacts;
        public ObservableCollection<ContactModel> Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
                OnPropertyChanged();
            }
        }

        public WebAPI WebAPI { get; set; }
        public IdentityUserModel User => WebAPI.User;
        public UserModel? SelectedUser;
        public bool SelectThisUser => SelectedUser == null || !User.IsAdmin || SelectedUser.Id == User.Id;
        public Guid SelectedUserId => SelectThisUser ? User.Id : SelectedUser?.Id ?? User.Id;
        public string SelectedUserName => (SelectThisUser ? User.Name : SelectedUser?.Name ?? User.Name) ?? "";

        public bool Available { get => User.IsAuthenticated && !User.InProcessAuthenticated; }
        public Visibility AdminPanelVisibility => User.IsAdmin ? Visibility.Visible : Visibility.Hidden;

        public RelayCommand UpdateCommand { get; }
        public RelayCommand ImportCommand { get; }
        public RelayCommand ExportCommand { get; }
        public RelayCommand GenerateCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand LoginCommand { get; }
        public RelayCommand LogoutCommand { get; }
        public RelayCommand CreateCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand UsersCommand { get; }
        public RelayCommand SelectUserCommand { get; }

        public ApplicationViewModel()
        {
            WebAPI = new();

            var launch = new LaunchManager(
                new() {
                    ExpectedAddress = "https://localhost:7058/"
                });
            launch.OnStart();

            contacts = new();
            
            LoginCommand = new(LoginCommandExecute, LoginCommandCanExecute);
            LogoutCommand = new(LogoutCommandExecute, LogoutCommandCanExecute,
                "Are you sure you want to logout?");

            UpdateCommand = new(UpdateCommandExecute, AvailableCommandCanExecute);
            ImportCommand = new(ImportCommandExecute, ReadOnlyCanExecute);
            ExportCommand = new(ExportCommandExecute, AvailableCommandCanExecute);

            ClearCommand = new(ClearCommandExecute, ReadOnlyCanExecute,
                "Are you sure you want to clear all contacts?");
            
            GenerateCommand = new(GenerateCommandExecute, ReadOnlyCanExecute,
                "Are you sure you want to generate test contacts?");

            CreateCommand = new(CreateCommandExecute, ReadOnlyCanExecute);
            EditCommand   = new(EditCommandExecute, ReadOnlyCanExecute);
            DeleteCommand = new(DeleteCommandExecute, ReadOnlyCanExecute,
                "Are you sure you want to delete contact?");

            UsersCommand = new(UsersCommandExecute, UsersCommandCanExecute);

            SelectUserCommand = new(SelectUserCommandExecute, AvailableCommandCanExecute);

            Initialize();
        }

        public async void CreateCommandExecute()
        {
            var contact = new ContactModel();
            
            var window = new ElementForm(contact);
            if (window.ShowDialog() != true)
                return;

            var createdId = await WebAPI.CreateContactAsync(contact);
            if (createdId == null)
                return;
            
            contact.Id = (Guid)createdId;
            
            await UpdateContactFromBaseAsync(contact);

            Contacts.Add(contact);
        }
        public async void UsersCommandExecute()
        {
            var users = await WebAPI.ListUsersAsync();
            if (users == null)
                return;

            var view = new UsersViewModel(this, users);
            var form = new UsersForm(view);
            form.ShowDialog();

        }

        public void SelectUserCommandExecute(object? param)
        {
            if (param == null)
                return;

            SelectedUser = (UserModel)param;
            OnPropertyChanged(nameof(SelectedUserName));
            OnPropertyChanged(nameof(SelectThisUser));
            UpdateCommandExecute();
        }
        
        public bool UsersCommandCanExecute()
        {
            return User.IsAuthenticated && User.IsAdmin && AvailableCommandCanExecute();
        }


        public async void DeleteCommandExecute(object? param)
        {
            if (param == null)
                return;

            var contact = (ContactModel)param;
            var deleted = await WebAPI.DeleteContactAsync(contact.Id);

            if (deleted)
                Contacts.Remove(contact);
            else
                await UpdateContactFromBaseAsync(contact);
        }
        public async void EditCommandExecute(object? param)
        {
            if (param == null)
                return;

            var contact  = (ContactModel)param;
            await UpdateContactFromBaseAsync(contact);

            var tempContact = new ContactModel();
            tempContact.Fill(contact);

            var window = new ElementForm(tempContact);
            if (window.ShowDialog() != true)
                return;

            contact.Fill(tempContact);

            await WebAPI.UpdateContact(contact);

            await UpdateContactFromBaseAsync(contact);
        }

        public async Task UpdateContactFromBaseAsync(ContactModel contact)
        {
            var contactBase = await WebAPI.GetContactAsync(contact.Id);
            if (contactBase != null)
                contact.Fill(contactBase);
        }

        public async void Initialize()
        {
            
            await WebAPI.LoginAsync();
            UpdateCommandExecute();
        }

        public async void UpdateCommandExecute()
        {
            ObservableCollection<ContactModel>? newContacts = null;
            if (AvailableCommandCanExecute())
            {
                try
                {
                    newContacts = (await WebAPI.ListContactsAsync(SelectedUserId))?.Contacts;
                }
                catch
                {

                }
                
            }                
            
            Contacts = newContacts ?? new();

            OnPropertyChanged("AdminPanelVisibility");

        }

        public bool AvailableCommandCanExecute()
        {
            return Available;
        }

        public bool AvailableCommandCanExecute(object? param = null)
        {
            return param != null && AvailableCommandCanExecute();
        }

        public async void ImportCommandExecute()
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = $"(*.json) | *.json;";
            if (fileDialog.ShowDialog() != true)
                return;

            var path = fileDialog.FileName;
            if (!File.Exists(path))
                return;

            string readText = File.ReadAllText(path);
            
            await WebAPI.ImportContacts(readText);

            UpdateCommandExecute();

        }

        public async void ExportCommandExecute()
        {            
            FileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = $"(*.json) | *.json;";
            if (fileDialog.ShowDialog() != true)
                return;
            
            var result = await WebAPI.ExportContacts();
            if (result == null)
                return;
            
            string path = Path.Combine(fileDialog.FileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
                result.CopyTo(outputFileStream);

        }
        public async void GenerateCommandExecute()
        {
            try
            {
                await WebAPI.GenerateContactsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            UpdateCommandExecute();
        }
        
        public bool ReadOnlyCanExecute()
        {
            return AvailableCommandCanExecute() && SelectThisUser;
        }
        public bool ReadOnlyCanExecute(object? param)
        {
            return AvailableCommandCanExecute(param) && SelectThisUser;
        }

        public async void ClearCommandExecute()
        {
            
            try
            {
                await WebAPI.ClearContactsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            UpdateCommandExecute();
        }
        public async void LoginCommandExecute()
        {
            await WebAPI.LoginAsync();

            UpdateCommandExecute();
        }

        public bool LoginCommandCanExecute()
        {
            return !User.InProcessAuthenticated;
        }

        public void LogoutCommandExecute()
        {
            WebAPI.Logout();

            UpdateCommandExecute();
        }

        public bool LogoutCommandCanExecute()
        {
            return User.IsAuthenticated && !User.InProcessAuthenticated;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
