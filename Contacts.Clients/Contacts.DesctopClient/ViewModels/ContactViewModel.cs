using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Contacts.Shared.Models;

namespace Contacts.DesctopClient.ViewModels;

public class ContactViewModel : ContactModel, INotifyPropertyChanged
{

    public Guid IdView
    {
        get { return Id; }
        set
        {
            Id = value;
            OnPropertyChanged();
        }
    }

    public string FirstNameView
    {
        get { return FirstName; }
        set
        {
            FirstName = value;
            OnPropertyChanged();
        }
    }

    public string? LastNameView
    {
        get { return LastName; }
        set
        {
            LastName = value;
            OnPropertyChanged();
        }
    }

    public string? MiddleNameView
    {
        get { return MiddleName; }
        set
        {
            MiddleName = value;
            OnPropertyChanged();
        }
    }

    public string? PhoneView
    {
        get { return Phone; }
        set
        {
            Phone = value;
            OnPropertyChanged();
        }
    }

    public string? EmailView
    {
        get { return Email; }
        set
        {
            Email = value;
            OnPropertyChanged();
        }
    }

    public string? DescriptionView
    {
        get { return Description; }
        set
        {
            Description = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }

    public void Fill(ContactViewModel contact)
    {
        this.FirstNameView = contact.FirstNameView;
        this.LastNameView = contact.LastNameView;
        this.MiddleNameView = contact.MiddleNameView;
        this.PhoneView = contact.PhoneView;
        this.EmailView = contact.EmailView;
        this.DescriptionView = contact.DescriptionView;
    }

}
