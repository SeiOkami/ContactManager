using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Contacts.DesctopClient.Models;

namespace Contacts.DesctopClient.ViewModels;

public class UsersViewModel : INotifyPropertyChanged
{
    private ObservableCollection<UserModel> users;
    public ObservableCollection<UserModel> Users
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

    public UsersViewModel(ApplicationViewModel appModel, List<UserModel> users)
    {
        this.AppModel = appModel;
        this.users = new(users);
    }

}
