using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ProjectManagementApp.Commands;
using ProjectManagementApp.Models;
using ProjectManagementApp.Repositories;
using ProjectManagementApp.Services;

namespace ProjectManagementApp.ViewModels
{
    public class HQViewModel : BaseViewModel, IDisposable
    {
        private readonly IStatusUpdateService _statusUpdateService;
        private bool _disposed;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private ObservableCollection<EmployeeViewModel> _baseEmployees;
        private ObservableCollection<EquipmentItemViewModel> _baseEquipment;
        private ObservableCollection<EmployeeViewModel> _filteredBaseEmployees;
        private ObservableCollection<EquipmentItemViewModel> _filteredBaseEquipment;
        private string _employeeSearchText;
        private string _equipmentSearchText;

        public ObservableCollection<EmployeeViewModel> BaseEmployees
        {
            get => _baseEmployees;
            set => SetProperty(ref _baseEmployees, value);
        }

        public ObservableCollection<EquipmentItemViewModel> BaseEquipment
        {
            get => _baseEquipment;
            set => SetProperty(ref _baseEquipment, value);
        }

        public ObservableCollection<EmployeeViewModel> FilteredBaseEmployees
        {
            get => _filteredBaseEmployees;
            set => SetProperty(ref _filteredBaseEmployees, value);
        }

        public ObservableCollection<EquipmentItemViewModel> FilteredBaseEquipment
        {
            get => _filteredBaseEquipment;
            set => SetProperty(ref _filteredBaseEquipment, value);
        }

        public string EmployeeSearchText
        {
            get => _employeeSearchText;
            set
            {
                if (SetProperty(ref _employeeSearchText, value))
                {
                    FilterEmployees();
                    OnPropertyChanged(nameof(EmployeeClearButtonVisibility));
                }
            }
        }

        public string EquipmentSearchText
        {
            get => _equipmentSearchText;
            set
            {
                if (SetProperty(ref _equipmentSearchText, value))
                {
                    FilterEquipment();
                    OnPropertyChanged(nameof(EquipmentClearButtonVisibility));
                }
            }
        }

        public Visibility EmployeeClearButtonVisibility =>
            string.IsNullOrWhiteSpace(EmployeeSearchText) ? Visibility.Collapsed : Visibility.Visible;

        public Visibility EquipmentClearButtonVisibility =>
            string.IsNullOrWhiteSpace(EquipmentSearchText) ? Visibility.Collapsed : Visibility.Visible;

        public ICommand ClearEmployeeSearchCommand { get; }
        public ICommand ClearEquipmentSearchCommand { get; }

        public HQViewModel(
            IEmployeeRepository employeeRepository,
            IEquipmentRepository equipmentRepository,
            IStatusUpdateService statusUpdateService)
        {
            _statusUpdateService = statusUpdateService;
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _equipmentRepository = equipmentRepository ?? throw new ArgumentNullException(nameof(equipmentRepository));

            _statusUpdateService.StatusUpdated += async (sender, args) =>
            {
                await LoadDataAsync();
            };

            BaseEmployees = new ObservableCollection<EmployeeViewModel>();
            BaseEquipment = new ObservableCollection<EquipmentItemViewModel>();
            FilteredBaseEmployees = new ObservableCollection<EmployeeViewModel>();
            FilteredBaseEquipment = new ObservableCollection<EquipmentItemViewModel>();

            ClearEmployeeSearchCommand = new RelayCommand(ExecuteClearEmployeeSearch);
            ClearEquipmentSearchCommand = new RelayCommand(ExecuteClearEquipmentSearch);

            _ = Task.Run(LoadDataAsync);
        }

        private async Task LoadDataAsync()
        {
            await Task.WhenAll(LoadEmployeesAsync(), LoadEquipmentAsync());
        }

        private async Task LoadEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BaseEmployees = new ObservableCollection<EmployeeViewModel>(
                    employees.Where(e => e.Status == "Baza")
                            .Select(e => new EmployeeViewModel(e)));
                FilterEmployees();
            });
        }

        private async Task LoadEquipmentAsync()
        {
            var equipment = await _equipmentRepository.GetAllEquipmentAsync();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BaseEquipment = new ObservableCollection<EquipmentItemViewModel>(
                    equipment.Where(e => e.Status == "Baza" || e.Status == "Serwis")
                            .Select(e => new EquipmentItemViewModel(e)));
                FilterEquipment();
            });
        }

        private void FilterEmployees()
        {
            if (string.IsNullOrWhiteSpace(EmployeeSearchText))
            {
                FilteredBaseEmployees = new ObservableCollection<EmployeeViewModel>(BaseEmployees);
                return;
            }

            var searchTerms = EmployeeSearchText.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var filteredList = BaseEmployees.Where(employee =>
                searchTerms.All(term =>
                    employee.FirstName.ToLower().Contains(term) ||
                    employee.LastName.ToLower().Contains(term) ||
                    employee.FullName.ToLower().Contains(term)))
                .ToList();

            FilteredBaseEmployees = new ObservableCollection<EmployeeViewModel>(filteredList);
        }

        private void FilterEquipment()
        {
            if (string.IsNullOrWhiteSpace(EquipmentSearchText))
            {
                FilteredBaseEquipment = new ObservableCollection<EquipmentItemViewModel>(BaseEquipment);
                return;
            }

            var searchTerms = EquipmentSearchText.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var filteredList = BaseEquipment.Where(equipment =>
                searchTerms.All(term =>
                    equipment.Name.ToLower().Contains(term)))
                .ToList();

            FilteredBaseEquipment = new ObservableCollection<EquipmentItemViewModel>(filteredList);
        }

        private void ExecuteClearEmployeeSearch()
        {
            EmployeeSearchText = string.Empty;
        }

        private void ExecuteClearEquipmentSearch()
        {
            EquipmentSearchText = string.Empty;
        }

        public async Task RefreshData()
        {
            await LoadDataAsync();
        }

        private async void StatusUpdateService_StatusUpdated(object sender, StatusUpdateEventArgs e)
        {
            await LoadDataAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _statusUpdateService.StatusUpdated -= StatusUpdateService_StatusUpdated;
                }
                _disposed = true;
            }
        }
    }
}