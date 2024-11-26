using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> AssignedEmployees { get; set; } = new List<Employee>();
        public ICollection<Equipment> AssignedEquipment { get; set; } = new List<Equipment>();
    }
}
