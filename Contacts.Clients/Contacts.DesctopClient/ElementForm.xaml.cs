using Contacts.DesctopClient.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Contacts.DesctopClient
{

    public partial class ElementForm : Window
    {

        private ContactViewModel element;

        private bool isCorrect
        {
            get
            {
                return !String.IsNullOrWhiteSpace(element.FirstName);
            }
        }
        
        public ElementForm(ContactViewModel element)
        {
            InitializeComponent();

            this.element = element;
            DataContext = element;
        }
        
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!isCorrect)
                MessageBox.Show($"There are empty fields!");
            else
                this.DialogResult = true;
        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        
        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
