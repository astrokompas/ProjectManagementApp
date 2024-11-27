using ProjectManagementApp.Models;
using ProjectManagementApp.Repositories;
using ProjectManagementApp.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ProjectManagementApp.Windows;

namespace ProjectManagementApp.ViewModels
{
    public class ProjectItemViewModel : BaseViewModel
    {
        private readonly Project _project;
        private readonly IProjectAssignmentService _assignmentService;
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEquipmentRepository _equipmentRepository;

        public ProjectItemViewModel(
            Project project,
            IProjectAssignmentService assignmentService,
            IProjectRepository projectRepository,
            IEmployeeRepository employeeRepository,
            IEquipmentRepository equipmentRepository)
        {
            _project = project;
            _assignmentService = assignmentService;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _equipmentRepository = equipmentRepository;

            InitializeCommands();
        }

        public int Id => _project.Id;
        public string Name
        {
            get => _project.Name;
            set
            {
                if (_project.Name != value)
                {
                    _project.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _project.Description;
            set
            {
                if (_project.Description != value)
                {
                    _project.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<EmployeeViewModel> AssignedEmployees { get; } = new();
        public ObservableCollection<EquipmentItemViewModel> AssignedEquipment { get; } = new();

        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand AssignEmployeesCommand { get; private set; }
        public ICommand AssignEquipmentCommand { get; private set; }
        public ICommand RemoveEmployeeCommand { get; private set; }
        public ICommand RemoveEquipmentCommand { get; private set; }

        private void InitializeCommands()
        {
            EditCommand = new AsyncRelayCommand(ExecuteEdit);
            DeleteCommand = new AsyncRelayCommand(ExecuteDelete);
            AssignEmployeesCommand = new AsyncRelayCommand(ExecuteAssignEmployees);
            AssignEquipmentCommand = new AsyncRelayCommand(ExecuteAssignEquipment);
            RemoveEmployeeCommand = new AsyncRelayCommand<int>(ExecuteRemoveEmployee);
            RemoveEquipmentCommand = new AsyncRelayCommand<int>(ExecuteRemoveEquipment);
        }

        private async Task ExecuteEdit()
        {
            var dialog = new EditProjectDialog(_project);
            if (dialog.ShowDialog() == true)
            {
                await _projectRepository.UpdateProjectAsync(_project);
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Description));
            }
        }

        private async Task ExecuteDelete()
        {
            var result = MessageBox.Show(
                "Are you sure you want to delete this project?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await _projectRepository.DeleteProjectAsync(Id);
                RequestRemoval?.Invoke(this, EventArgs.Empty);
            }
        }

        private async Task ExecuteAssignEmployees()
        {
            var dialog = new AssignResourcesDialog(
                ResourceType.Employee,
                Id,
                _assignmentService,
                _employeeRepository,
                _equipmentRepository);

            var result = dialog.ShowDialog();
            if (result == true)
            {
                await RefreshAssignedEmployees();
            }
        }

        private async Task ExecuteAssignEquipment()
        {
            var dialog = new AssignResourcesDialog(
                ResourceType.Equipment,
                Id,
                _assignmentService,
                _employeeRepository,
                _equipmentRepository);

            var result = dialog.ShowDialog();
            if (result == true)
            {
                await RefreshAssignedEquipment();
            }
        }

        private async Task ExecuteRemoveEmployee(int employeeId)
        {
            await _assignmentService.RemoveEmployeeFromProject(employeeId);
            await RefreshAssignedEmployees();
        }

        private async Task ExecuteRemoveEquipment(int equipmentId)
        {
            await _assignmentService.RemoveEquipmentFromProject(equipmentId);
            await RefreshAssignedEquipment();
        }

        private async Task RefreshAssignedEmployees()
        {
            var project = await _projectRepository.GetProjectByIdAsync(Id);
            AssignedEmployees.Clear();
            foreach (var employee in project.AssignedEmployees)
            {
                AssignedEmployees.Add(new EmployeeViewModel(employee));
            }
        }

        private async Task RefreshAssignedEquipment()
        {
            var project = await _projectRepository.GetProjectByIdAsync(Id);
            AssignedEquipment.Clear();
            foreach (var equipment in project.AssignedEquipment)
            {
                AssignedEquipment.Add(new EquipmentItemViewModel(equipment));
            }
        }

        public event EventHandler RequestRemoval;
    }
}
