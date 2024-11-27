using System.Windows.Media;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.ViewModels
{
    public class EquipmentItemViewModel : BaseViewModel
    {
        private readonly Equipment _equipment;

        public int Id => _equipment.Id;
        public string Name => _equipment.Name;
        public string Status => _equipment.Status;

        public string StatusDetails
        {
            get
            {
                return Status == "Na Robocie" && _equipment.Project != null
                    ? $"(Projekt: {_equipment.Project.Name})"
                    : "";
            }
        }

        public Brush StatusColor
        {
            get
            {
                return Status switch
                {
                    "Baza" => Brushes.LightGreen,
                    "Na Robocie" => Brushes.Orange,
                    "Serwis" => Brushes.Red,
                    _ => Brushes.White
                };
            }
        }

        public EquipmentItemViewModel(Equipment equipment)
        {
            _equipment = equipment;
        }
    }
}