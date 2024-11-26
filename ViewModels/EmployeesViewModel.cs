using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using ProjectManagementApp.Models;
using ProjectManagementApp.Repositories;
using ProjectManagementApp.Windows;
using ProjectManagementApp.Commands;
using ProjectManagementApp.Services;
using System.Windows;
using System.Threading.Tasks;

namespace ProjectManagementApp.ViewModels
{
    public class EmployeesViewModel : BaseViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProjectAssignmentService _projectAssignmentService;
        private ObservableCollection<EmployeeViewModel> _employees;
        private string _searchText;
        private ObservableCollection<EmployeeViewModel> _filteredEmployees;

        public ObservableCollection<EmployeeViewModel> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        public ObservableCollection<EmployeeViewModel> FilteredEmployees
        {
            get => _filteredEmployees;
            set => SetProperty(ref _filteredEmployees, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterEmployees();
                    OnPropertyChanged(nameof(ClearButtonVisibility));
                }
            }
        }

        public Visibility ClearButtonVisibility =>
            string.IsNullOrWhiteSpace(SearchText) ? Visibility.Collapsed : Visibility.Visible;

        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand ChangeStatusCommand { get; }
        public ICommand ClearSearchCommand { get; }

        public EmployeesViewModel(
            IEmployeeRepository employeeRepository,
            IProjectAssignmentService projectAssignmentService)
        {
            try
            {
                _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
                _projectAssignmentService = projectAssignmentService ?? throw new ArgumentNullException(nameof(projectAssignmentService));

                Employees = new ObservableCollection<EmployeeViewModel>();
                FilteredEmployees = new ObservableCollection<EmployeeViewModel>();

                AddEmployeeCommand = new RelayCommand(ExecuteAddEmployee);
                EditEmployeeCommand = new RelayCommand<EmployeeViewModel>(ExecuteEditEmployee);
                DeleteEmployeeCommand = new RelayCommand<EmployeeViewModel>(ExecuteDeleteEmployee);
                ChangeStatusCommand = new RelayCommand<EmployeeViewModel>(ExecuteChangeStatus);
                ClearSearchCommand = new RelayCommand(ExecuteClearSearch);

                _ = Task.Run(LoadEmployeesAsync);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ViewModel constructor: {ex.Message}");
            }
        }

        private void FilterEmployees()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredEmployees = new ObservableCollection<EmployeeViewModel>(Employees);
                return;
            }

            var searchTerms = SearchText.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var filteredList = Employees.Where(employee =>
                searchTerms.All(term =>
                    employee.FirstName.ToLower().Contains(term) ||
                    employee.LastName.ToLower().Contains(term) ||
                    employee.FullName.ToLower().Contains(term)))
                .ToList();

            FilteredEmployees = new ObservableCollection<EmployeeViewModel>(filteredList);
        }

        private void ExecuteClearSearch()
        {
            SearchText = string.Empty;
        }

        private async Task LoadEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Employees = new ObservableCollection<EmployeeViewModel>(
                    employees.Select(e => new EmployeeViewModel(e)));
                FilterEmployees();
            });
        }

        private async void ExecuteAddEmployee()
        {
            try
            {
                var dialog = new EmployeeDialog();
                var viewModel = new EmployeeDialogViewModel(
                    "Dodaj pracownika",
                    _employeeRepository);  // Pass the repository
                dialog.DataContext = viewModel;
                dialog.Owner = Application.Current.MainWindow;

                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        var newEmployee = await _employeeRepository.AddEmployeeAsync(new Employee
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            Status = "Baza"
                        });

                        var newViewModel = new EmployeeViewModel(newEmployee);
                        Employees.Add(newViewModel);
                        FilterEmployees();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding employee: {ex.Message}");
                        var errorDialog = new ErrorDialog(ex.Message);
                        errorDialog.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Outer error: {ex.Message}");
            }
        }

        private async void ExecuteEditEmployee(EmployeeViewModel employee)
        {
            var dialog = new EmployeeDialog();
            var viewModel = new EmployeeDialogViewModel(
                "Edytuj pracownika",
                _employeeRepository,
                employee.FirstName,
                employee.LastName);
            dialog.DataContext = viewModel;

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var updatedEmployee = await _employeeRepository.GetEmployeeByIdAsync(employee.Id);
                    updatedEmployee.FirstName = viewModel.FirstName;
                    updatedEmployee.LastName = viewModel.LastName;

                    await _employeeRepository.UpdateEmployeeAsync(updatedEmployee);
                    await LoadEmployeesAsync();
                }
                catch (Exception ex)
                {
                    var errorDialog = new ErrorDialog(ex.Message);
                    errorDialog.ShowDialog();
                }
            }
        }

        private async void ExecuteDeleteEmployee(EmployeeViewModel employee)
        {
            var confirmDialog = new ConfirmationDialog(
                "Usuń pracownika",
                "Czy na pewno chcesz usunąć tego pracownika?");

            if (confirmDialog.ShowDialog() == true)
            {
                await _employeeRepository.DeleteEmployeeAsync(employee.Id);
                Employees.Remove(employee);
                FilterEmployees();
            }
        }

        private async void ExecuteChangeStatus(EmployeeViewModel employee)
        {
            var dialog = new EmployeeStatusDialog(employee);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    await _projectAssignmentService.UpdateEmployeeStatus(
                        employee.Id,
                        dialog.SelectedStatus,
                        dialog.SelectedProjectId,
                        dialog.VacationStart,
                        dialog.VacationEnd);

                    await LoadEmployeesAsync();
                }
                catch (Exception ex)
                {
                    var errorDialog = new ErrorDialog(ex.Message);
                    errorDialog.ShowDialog();
                }
            }
        }
    }
}