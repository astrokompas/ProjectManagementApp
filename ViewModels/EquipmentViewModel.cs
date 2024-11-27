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
using ProjectManagementApp.Windows;

namespace ProjectManagementApp.ViewModels
{
    public class EquipmentViewModel : BaseViewModel
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IProjectAssignmentService _projectAssignmentService;
        private ObservableCollection<EquipmentItemViewModel> _equipment;
        private string _searchText;
        private ObservableCollection<EquipmentItemViewModel> _filteredEquipment;

        public ObservableCollection<EquipmentItemViewModel> Equipment
        {
            get => _equipment;
            set => SetProperty(ref _equipment, value);
        }

        public ObservableCollection<EquipmentItemViewModel> FilteredEquipment
        {
            get => _filteredEquipment;
            set => SetProperty(ref _filteredEquipment, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterEquipment();
                    OnPropertyChanged(nameof(ClearButtonVisibility));
                }
            }
        }

        public Visibility ClearButtonVisibility =>
            string.IsNullOrWhiteSpace(SearchText) ? Visibility.Collapsed : Visibility.Visible;

        public ICommand AddEquipmentCommand { get; }
        public ICommand EditEquipmentCommand { get; }
        public ICommand DeleteEquipmentCommand { get; }
        public ICommand ChangeStatusCommand { get; }
        public ICommand ClearSearchCommand { get; }

        public EquipmentViewModel(
            IEquipmentRepository equipmentRepository,
            IProjectAssignmentService projectAssignmentService)
        {
            _equipmentRepository = equipmentRepository;
            _projectAssignmentService = projectAssignmentService;

            Equipment = new ObservableCollection<EquipmentItemViewModel>();
            FilteredEquipment = new ObservableCollection<EquipmentItemViewModel>();

            AddEquipmentCommand = new RelayCommand(ExecuteAddEquipment);
            EditEquipmentCommand = new RelayCommand<EquipmentItemViewModel>(ExecuteEditEquipment);
            DeleteEquipmentCommand = new RelayCommand<EquipmentItemViewModel>(ExecuteDeleteEquipment);
            ChangeStatusCommand = new RelayCommand<EquipmentItemViewModel>(ExecuteChangeStatus);
            ClearSearchCommand = new RelayCommand(ExecuteClearSearch);

            _ = Task.Run(LoadEquipmentAsync);
        }

        private void FilterEquipment()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredEquipment = new ObservableCollection<EquipmentItemViewModel>(Equipment);
                return;
            }

            var searchTerms = SearchText.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var filteredList = Equipment.Where(equipment =>
                searchTerms.All(term =>
                    equipment.Name.ToLower().Contains(term)))
                .ToList();

            FilteredEquipment = new ObservableCollection<EquipmentItemViewModel>(filteredList);
        }

        private void ExecuteClearSearch()
        {
            SearchText = string.Empty;
        }

        private async Task LoadEquipmentAsync()
        {
            var equipment = await _equipmentRepository.GetAllEquipmentAsync();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Equipment = new ObservableCollection<EquipmentItemViewModel>(
                    equipment.Select(e => new EquipmentItemViewModel(e)));
                FilterEquipment();
            });
        }

        private async void ExecuteAddEquipment()
        {
            var dialog = new EquipmentDialog();
            var viewModel = new EquipmentDialogViewModel(
                "Dodaj sprzęt",
                _equipmentRepository);
            dialog.DataContext = viewModel;
            dialog.Owner = Application.Current.MainWindow;

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var newEquipment = await _equipmentRepository.AddEquipmentAsync(new Equipment
                    {
                        Name = viewModel.Name,
                        Status = "Baza"
                    });

                    var newViewModel = new EquipmentItemViewModel(newEquipment);
                    Equipment.Add(newViewModel);
                    FilterEquipment();
                }
                catch (Exception ex)
                {
                    var errorDialog = new ErrorDialog(ex.Message);
                    errorDialog.ShowDialog();
                }
            }
        }

        private async void ExecuteEditEquipment(EquipmentItemViewModel equipment)
        {
            var dialog = new EquipmentDialog();
            var viewModel = new EquipmentDialogViewModel(
                "Edytuj sprzęt",
                _equipmentRepository,
                equipment.Name);
            dialog.DataContext = viewModel;

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var updatedEquipment = await _equipmentRepository.GetEquipmentByIdAsync(equipment.Id);
                    updatedEquipment.Name = viewModel.Name;

                    await _equipmentRepository.UpdateEquipmentAsync(updatedEquipment);
                    await LoadEquipmentAsync();
                }
                catch (Exception ex)
                {
                    var errorDialog = new ErrorDialog(ex.Message);
                    errorDialog.ShowDialog();
                }
            }
        }

        private async void ExecuteDeleteEquipment(EquipmentItemViewModel equipment)
        {
            var confirmDialog = new ConfirmationDialog(
                "Usuń sprzęt",
                "Czy na pewno chcesz usunąć ten sprzęt?");

            if (confirmDialog.ShowDialog() == true)
            {
                await _equipmentRepository.DeleteEquipmentAsync(equipment.Id);
                Equipment.Remove(equipment);
                FilterEquipment();
            }
        }

        private async void ExecuteChangeStatus(EquipmentItemViewModel equipment)
        {
            var dialog = new EquipmentStatusDialog(equipment);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    await _projectAssignmentService.UpdateEquipmentStatus(
                        equipment.Id,
                        dialog.SelectedStatus,
                        dialog.SelectedProjectId);

                    await LoadEquipmentAsync();
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