using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ProjectManagementApp.Commands;
using ProjectManagementApp.Validators;

namespace ProjectManagementApp.ViewModels
{
    public class ContactDialogViewModel : BaseViewModel
    {
        private string _email;
        private string _dialogTitle;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string DialogTitle
        {
            get => _dialogTitle;
            set => SetProperty(ref _dialogTitle, value);
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public ContactDialogViewModel(string title, string email = "")
        {
            DialogTitle = title;
            Email = email;

            ConfirmCommand = new RelayCommand<Window>(ExecuteConfirm);
            CancelCommand = new RelayCommand<Window>(ExecuteCancel);
        }

        private void ExecuteConfirm(Window window)
        {
            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void ExecuteCancel(Window window)
        {
            if (window != null)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}
