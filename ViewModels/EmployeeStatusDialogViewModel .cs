using ProjectManagementApp.Models;
using ProjectManagementApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectManagementApp.ViewModels
{
    public class EmployeeStatusDialogViewModel : BaseViewModel
    {
        private readonly IProjectRepository _projectRepository;
        private string _selectedStatus;
        private Project _selectedProject;
        private DateTime? _vacationStart;
        private DateTime? _vacationEnd;

        public ObservableCollection<string> AvailableStatuses { get; } = new ObservableCollection<string>
    {
        "Baza",
        "Na Robocie",
        "Urlop"
    };

        public ObservableCollection<Project> AvailableProjects { get; private set; }

        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SetProperty(ref _selectedStatus, value))
                {
                    OnPropertyChanged(nameof(ProjectSelectionVisibility));
                    OnPropertyChanged(nameof(VacationDatesVisibility));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public Project SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (SetProperty(ref _selectedProject, value))
                    OnPropertyChanged(nameof(CanSave));
            }
        }

        public DateTime? VacationStart
        {
            get => _vacationStart;
            set
            {
                if (SetProperty(ref _vacationStart, value))
                    OnPropertyChanged(nameof(CanSave));
            }
        }

        public DateTime? VacationEnd
        {
            get => _vacationEnd;
            set
            {
                if (SetProperty(ref _vacationEnd, value))
                    OnPropertyChanged(nameof(CanSave));
            }
        }

        public Visibility ProjectSelectionVisibility =>
            SelectedStatus == "Na Robocie" ? Visibility.Visible : Visibility.Collapsed;

        public Visibility VacationDatesVisibility =>
            SelectedStatus == "Urlop" ? Visibility.Visible : Visibility.Collapsed;

        public bool CanSave => SelectedStatus switch
        {
            "Na Robocie" => SelectedProject != null,
            "Urlop" => VacationStart.HasValue && VacationEnd.HasValue && VacationStart < VacationEnd,
            _ => true
        };

        public EmployeeStatusDialogViewModel(EmployeeViewModel employee, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            SelectedStatus = employee.Status;
            LoadProjects();
        }

        private async void LoadProjects()
        {
            var projects = await _projectRepository.GetAllProjectsAsync();
            AvailableProjects = new ObservableCollection<Project>(projects);
            OnPropertyChanged(nameof(AvailableProjects));
        }

        public bool CanExecuteSave() => CanSave;
    }
}
