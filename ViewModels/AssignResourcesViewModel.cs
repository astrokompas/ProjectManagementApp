using ProjectManagementApp.Commands;
using ProjectManagementApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ProjectManagementApp.Windows;

namespace ProjectManagementApp.ViewModels
{
    public enum ResourceType
    {
        Employee,
        Equipment
    }

    public class ResourceItemViewModel : BaseViewModel
    {
        private bool _isSelected;
        private bool _isAvailable;

        public int Id { get; }
        public string Name { get; }
        public string Status { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }

        public ResourceItemViewModel(int id, string name, string status)
        {
            Id = id;
            Name = name;
            Status = status;
            IsAvailable = status == "Baza";
        }
    }

    public class AssignResourcesViewModel : BaseViewModel
    {
        public ICommand ToggleSelectionCommand { get; }

        private readonly ResourceType _resourceType;
        private readonly int _projectId;
        private readonly IProjectAssignmentService _assignmentService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEquipmentRepository _equipmentRepository;

        private string _searchText;
        public ObservableCollection<ResourceItemViewModel> FilteredResources { get; } = new();

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ApplyFilter();
                }
            }
        }

        public ObservableCollection<ResourceItemViewModel> Resources { get; } = new();
        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public AssignResourcesViewModel(
            ResourceType resourceType,
            int projectId,
            IProjectAssignmentService assignmentService,
            IEmployeeRepository employeeRepository,
            IEquipmentRepository equipmentRepository)
        {
            _resourceType = resourceType;
            _projectId = projectId;
            _assignmentService = assignmentService;
            _employeeRepository = employeeRepository;
            _equipmentRepository = equipmentRepository;

            ConfirmCommand = new AsyncRelayCommand(ExecuteConfirm);
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));
            ToggleSelectionCommand = new RelayCommand<ResourceItemViewModel>(ExecuteToggleSelection);

            _ = LoadResourcesAsync();
        }

        private async Task LoadResourcesAsync()
        {
            await LoadResources();
        }

        private void ExecuteToggleSelection(ResourceItemViewModel resource)
        {
            if (resource != null && resource.IsAvailable)
            {
                resource.IsSelected = !resource.IsSelected;
            }
        }

        private void ApplyFilter()
        {
            FilteredResources.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? Resources
                : Resources.Where(r =>
                    r.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var resource in filtered)
            {
                FilteredResources.Add(resource);
            }
        }

        private async Task LoadResources()
        {
            Resources.Clear();
            try
            {
                if (_resourceType == ResourceType.Employee)
                {
                    var employees = await _employeeRepository.GetAllEmployeesAsync();

                    var availableEmployees = employees
                        .Where(e => e.Status == "Baza")
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName);

                    var unavailableEmployees = employees
                        .Where(e => e.Status != "Baza")
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName);

                    foreach (var employee in availableEmployees.Concat(unavailableEmployees))
                    {
                        Resources.Add(new ResourceItemViewModel(
                            employee.Id,
                            $"{employee.FirstName} {employee.LastName}",
                            employee.Status)
                        {
                            IsAvailable = employee.Status == "Baza"
                        });
                    }
                }
                else
                {
                    var equipment = await _equipmentRepository.GetAllEquipmentAsync();

                    var availableEquipment = equipment
                        .Where(e => e.Status == "Baza")
                        .OrderBy(e => e.Name);

                    var unavailableEquipment = equipment
                        .Where(e => e.Status != "Baza")
                        .OrderBy(e => e.Name);

                    foreach (var item in availableEquipment.Concat(unavailableEquipment))
                    {
                        Resources.Add(new ResourceItemViewModel(
                            item.Id,
                            item.Name,
                            item.Status)
                        {
                            IsAvailable = item.Status == "Baza"
                        });
                    }
                }

                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading resources: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExecuteConfirm()
        {
            var selectedIds = Resources
                .Where(r => r.IsSelected && r.IsAvailable)
                .Select(r => r.Id)
                .ToList();

            foreach (var id in selectedIds)
            {
                if (_resourceType == ResourceType.Employee)
                {
                    await _assignmentService.AssignEmployeeToProject(id, _projectId);
                }
                else
                {
                    await _assignmentService.AssignEquipmentToProject(id, _projectId);
                }
            }

            RequestClose?.Invoke(true);
        }

        public event Action<bool> RequestClose;
    }
}
