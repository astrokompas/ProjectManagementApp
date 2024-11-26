using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ProjectManagementApp.Models;
using ProjectManagementApp.Repositories;
using System.Threading;
using System.Diagnostics;

namespace ProjectManagementApp.ViewModels
{
    public class EmployeeDialogViewModel : BaseViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private string _title;
        private string _firstName;
        private string _lastName;
        private string _firstNameError;
        private string _lastNameError;
        private CancellationTokenSource _validationCancellation;
        private readonly object _lockObject = new object();

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                {
                    ValidateFirstName();
                    StartDuplicateValidation();
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                {
                    ValidateLastName();
                    StartDuplicateValidation();
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public string FirstNameError
        {
            get => _firstNameError;
            set => SetProperty(ref _firstNameError, value);
        }

        public string LastNameError
        {
            get => _lastNameError;
            set => SetProperty(ref _lastNameError, value);
        }

        public bool CanSave => string.IsNullOrWhiteSpace(FirstNameError) &&
                              string.IsNullOrWhiteSpace(LastNameError) &&
                              !string.IsNullOrWhiteSpace(FirstName) &&
                              !string.IsNullOrWhiteSpace(LastName);

        public EmployeeDialogViewModel(string title, IEmployeeRepository employeeRepository, string firstName = "", string lastName = "")
        {
            _employeeRepository = employeeRepository;
            Title = title;
            FirstName = firstName;
            LastName = lastName;

            ValidateFirstName();
            ValidateLastName();
        }

        private void ValidateFirstName()
        {
            FirstNameError = string.IsNullOrWhiteSpace(FirstName) ? "Imię jest wymagane" : null;
        }

        private void ValidateLastName()
        {
            LastNameError = string.IsNullOrWhiteSpace(LastName) ? "Nazwisko jest wymagane" : null;
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

                if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
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
            if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync();

                cancellationToken.ThrowIfCancellationRequested();

                var isDuplicate = employees.Any(e =>
                    e.FirstName.Equals(FirstName, StringComparison.OrdinalIgnoreCase) &&
                    e.LastName.Equals(LastName, StringComparison.OrdinalIgnoreCase));

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (isDuplicate)
                    {
                        LastNameError = "Pracownik o takim imieniu i nazwisku już istnieje";
                    }
                    else if (LastNameError == "Pracownik o takim imieniu i nazwisku już istnieje")
                    {
                        LastNameError = null;
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