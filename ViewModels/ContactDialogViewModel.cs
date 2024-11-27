using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ProjectManagementApp.Commands;
using ProjectManagementApp.Validators;
using ProjectManagementApp.Repositories;
using System.Threading;
using System.Diagnostics;

namespace ProjectManagementApp.ViewModels
{
    public class ContactDialogViewModel : BaseViewModel
    {
        private readonly IContactRepository _contactRepository;
        private string _title;
        private string _email;
        private string _emailError;
        private CancellationTokenSource _validationCancellation;
        private readonly object _lockObject = new object();

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    ValidateEmail();
                    StartDuplicateValidation();
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }

        public bool CanSave => string.IsNullOrWhiteSpace(EmailError) &&
                              !string.IsNullOrWhiteSpace(Email);

        public ContactDialogViewModel(string title, IContactRepository contactRepository, string email = "")
        {
            _contactRepository = contactRepository;
            Title = title;
            Email = email;

            ValidateEmail();
        }

        private void ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email jest wymagany";
            }
            else if (!EmailValidator.IsValidEmail(Email))
            {
                EmailError = "Nieprawidłowy format adresu email";
            }
            else
            {
                EmailError = null;
            }
        }

        private async void StartDuplicateValidation()
        {
            try
            {
                if (_validationCancellation != null)
                {
                    _validationCancellation.Cancel();
                    _validationCancellation.Dispose();
                    _validationCancellation = null;
                }

                if (string.IsNullOrWhiteSpace(Email))
                {
                    return;
                }

                _validationCancellation = new CancellationTokenSource();
                var token = _validationCancellation.Token;

                await Task.Delay(500, token);

                if (!token.IsCancellationRequested)
                {
                    await ValidateDuplicateAsync(token);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Validation error: {ex.Message}");
            }
        }

        private async Task ValidateDuplicateAsync(CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(Email))
            {
                var contacts = await _contactRepository.GetAllContactsAsync();

                cancellationToken.ThrowIfCancellationRequested();

                var isDuplicate = contacts.Any(c =>
                    c.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (isDuplicate)
                    {
                        EmailError = "Kontakt o takim adresie email już istnieje";
                    }
                    else if (EmailError == "Kontakt o takim adresie email już istnieje")
                    {
                        EmailError = null;
                    }
                    OnPropertyChanged(nameof(CanSave));
                });
            }
        }

        public void Cleanup()
        {
            if (_validationCancellation != null)
            {
                _validationCancellation.Cancel();
                _validationCancellation.Dispose();
                _validationCancellation = null;
            }
        }
    }
}