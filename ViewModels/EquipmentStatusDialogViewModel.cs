using System.Collections.ObjectModel;
using System.Windows;
using ProjectManagementApp.Models;
using ProjectManagementApp.Repositories;

namespace ProjectManagementApp.ViewModels
{
    public class EquipmentStatusDialogViewModel : BaseViewModel
    {
        private readonly IProjectRepository _projectRepository;
        private string _selectedStatus;
        private Project _selectedProject;

        public ObservableCollection<string> AvailableStatuses { get; } = new ObservableCollection<string>
        {
            "Baza",
            "Na Robocie",
            "Serwis"
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

        public Visibility ProjectSelectionVisibility =>
            SelectedStatus == "Na Robocie" ? Visibility.Visible : Visibility.Collapsed;

        public bool CanSave => SelectedStatus switch
        {
            "Na Robocie" => SelectedProject != null,
            _ => true
        };

        public EquipmentStatusDialogViewModel(EquipmentItemViewModel equipment, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            SelectedStatus = equipment.Status;
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