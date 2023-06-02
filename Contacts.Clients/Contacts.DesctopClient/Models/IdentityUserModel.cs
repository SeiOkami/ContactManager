﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.DesctopClient.Models
{
    public class IdentityUserModel : INotifyPropertyChanged
    {
        private string? name;
        private string? token;
        private bool isAuthenticated = false;
        private bool inProcessAuthenticated = false;
        private bool isAdmin = false;
        private Guid id = Guid.Empty;

        public string? Name
        {
            get { return name; }
            set
            {
                name = value;
                Properties.Settings.Default.UserName = name;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public string? Token
        {
            get { return token; }
            set
            {
                token = value;
                Properties.Settings.Default.UserToken = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set
            {
                isAuthenticated = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }

        public Guid Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public bool InProcessAuthenticated
        {
            get { return inProcessAuthenticated; }
            set
            {
                inProcessAuthenticated = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
