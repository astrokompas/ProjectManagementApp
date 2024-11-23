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
        private string _errorMessage;

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                ValidateEmail();
                (ConfirmCommand as RelayCommand<Window>)?.RaiseCanExecuteChanged();
            }
        }

        public string DialogTitle
        {
            get => _dialogTitle;
            set => SetProperty(ref _dialogTitle, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public ContactDialogViewModel(string title, string email = "")
        {
            DialogTitle = title;
            Email = email;

            ConfirmCommand = new RelayCommand<Window>(ExecuteConfirm, CanConfirm);
            CancelCommand = new RelayCommand<Window>(ExecuteCancel);
        }

        private void ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Email jest wymagany";
            }
            else if (!EmailValidator.IsValidEmail(Email))
            {
                ErrorMessage = "Nieprawidłowy format adresu email";
            }
            else
            {
                ErrorMessage = null;
            }
        }

        private bool CanConfirm(Window window)
        {
            return string.IsNullOrEmpty(ErrorMessage) && !string.IsNullOrWhiteSpace(Email);
        }

        private void ExecuteConfirm(Window window)
        {
            if (window != null && CanConfirm(window))
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
