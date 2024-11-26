using System;
using System.Windows.Media;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        private readonly Employee _employee;

        public int Id => _employee.Id;
        public string FirstName => _employee.FirstName;
        public string LastName => _employee.LastName;
        public string FullName => $"{FirstName} {LastName}";
        public string Status => _employee.Status;

        public string StatusDetails
        {
            get
            {
                switch (Status)
                {
                    case "Na Robocie":
                        return _employee.Project != null
                            ? $"(Projekt: {_employee.Project.Name})"
                            : "";
                    case "Urlop":
                        return _employee.VacationStart.HasValue && _employee.VacationEnd.HasValue
                            ? $"({_employee.VacationStart.Value:dd.MM} - {_employee.VacationEnd.Value:dd.MM})"
                            : "";
                    default:
                        return "";
                }
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
                    "Urlop" => Brushes.LightBlue,
                    _ => Brushes.White
                };
            }
        }

        public EmployeeViewModel(Employee employee)
        {
            _employee = employee;
        }
    }
}