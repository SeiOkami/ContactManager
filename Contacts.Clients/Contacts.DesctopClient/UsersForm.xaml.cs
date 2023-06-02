using Contacts.DesctopClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Contacts.DesctopClient;

/// <summary>
/// Interaction logic for UsersForm.xaml
/// </summary>
public partial class UsersForm : Window
{
    private UsersViewModel model;
    public UsersForm(UsersViewModel model)
    {
        InitializeComponent();
        this.model = model;
        DataContext = model;
    }
}
