using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Contacts.Shared.Models;

namespace Contacts.DesctopClient.ViewModels;

public class UsersViewModel : INotifyPropertyChanged
{
    private ObservableCollection<UserInfoModel> users;
    public ObservableCollection<UserInfoModel> Users
    {
        get { return users; }
        set
        {
            users = value;
            OnPropertyChanged();
        }
    }
    public ApplicationViewModel AppModel;

    public RelayCommand SelectUserCommand => AppModel.SelectUserCommand;

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }

    public UsersViewModel(ApplicationViewModel appModel, List<UserInfoModel> users)
    {
        this.AppModel = appModel;
        this.users = new(users);
    }

}
