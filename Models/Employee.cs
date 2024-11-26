using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public DateTime? VacationStart { get; set; }
        public DateTime? VacationEnd { get; set; }
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
