using ProjectManagementApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectManagementApp.ViewModels
{
    public class EquipmentDialogViewModel : BaseViewModel
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private string _title;
        private string _name;
        private string _nameError;
        private CancellationTokenSource _validationCancellation;
        private readonly object _lockObject = new object();

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    ValidateName();
                    StartDuplicateValidation();
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public string NameError
        {
            get => _nameError;
            set => SetProperty(ref _nameError, value);
        }

        public bool CanSave => string.IsNullOrWhiteSpace(NameError) &&
                              !string.IsNullOrWhiteSpace(Name);

        public EquipmentDialogViewModel(string title, IEquipmentRepository equipmentRepository, string name = "")
        {
            _equipmentRepository = equipmentRepository;
            Title = title;
            Name = name;
            ValidateName();
        }

        private void ValidateName()
        {
            NameError = string.IsNullOrWhiteSpace(Name) ? "Nazwa jest wymagana" : null;
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

                if (string.IsNullOrWhiteSpace(Name))
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
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var equipment = await _equipmentRepository.GetAllEquipmentAsync();

                cancellationToken.ThrowIfCancellationRequested();

                var isDuplicate = equipment.Any(e =>
                    e.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (isDuplicate)
                    {
                        NameError = "Sprzęt o takiej nazwie już istnieje";
                    }
                    else if (NameError == "Sprzęt o takiej nazwie już istnieje")
                    {
                        NameError = null;
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
