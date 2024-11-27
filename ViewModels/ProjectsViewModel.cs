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
using ProjectManagementApp.Windows;

namespace ProjectManagementApp.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAssignmentService _assignmentService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private string _searchText;

        public ObservableCollection<ProjectItemViewModel> Projects { get; } = new();
        public ObservableCollection<ProjectItemViewModel> FilteredProjects { get; } = new();

        public ICommand AddProjectCommand { get; }

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

        public ProjectsViewModel(
            IProjectRepository projectRepository,
            IProjectAssignmentService assignmentService,
            IEmployeeRepository employeeRepository,
            IEquipmentRepository equipmentRepository)
        {
            _projectRepository = projectRepository;
            _assignmentService = assignmentService;
            _employeeRepository = employeeRepository;
            _equipmentRepository = equipmentRepository;

            AddProjectCommand = new AsyncRelayCommand(ExecuteAddProject);

            LoadProjects();
        }

        private ProjectItemViewModel CreateProjectItemViewModel(Project project)
        {
            return new ProjectItemViewModel(
                project,
                _assignmentService,
                _projectRepository,
                _employeeRepository,
                _equipmentRepository);
        }

        private async Task LoadProjects()
        {
            var projects = await _projectRepository.GetAllProjectsAsync();
            Projects.Clear();

            foreach (var project in projects)
            {
                var projectVm = CreateProjectItemViewModel(project);
                projectVm.RequestRemoval += OnProjectRequestRemoval;
                Projects.Add(projectVm);
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            FilteredProjects.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? Projects
                : Projects.Where(p =>
                    p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    p.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true);

            foreach (var project in filtered)
            {
                FilteredProjects.Add(project);
            }
        }

        private async Task ExecuteAddProject()
        {
            var dialog = new AddProjectDialog();
            if (dialog.ShowDialog() == true)
            {
                var newProject = new Project
                {
                    Name = dialog.ProjectName,
                    Description = dialog.ProjectDescription
                };

                await _projectRepository.AddProjectAsync(newProject);
                var projectVm = CreateProjectItemViewModel(newProject);
                projectVm.RequestRemoval += OnProjectRequestRemoval;
                Projects.Add(projectVm);
                ApplyFilter();
            }
        }

        private void OnProjectRequestRemoval(object sender, EventArgs e)
        {
            if (sender is ProjectItemViewModel projectVm)
            {
                projectVm.RequestRemoval -= OnProjectRequestRemoval;
                Projects.Remove(projectVm);
                ApplyFilter();
            }
        }
    }
}
