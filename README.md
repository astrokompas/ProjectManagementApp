# **Project Management Application**

A **WPF desktop application** designed for project management, built using the **MVVM architectural pattern** with **Entity Framework Core** for database operations. The application features both contact and employee management systems with comprehensive validation, status tracking, and database persistence.

---

## **🛠️ Technical Stack**

- **Framework**: .NET Core / WPF  
- **Architecture**: MVVM  
- **Database**: Microsoft SQL Server  
- **ORM**: Entity Framework Core  
- **Design Pattern**: Repository Pattern  
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection  

---

## **✨ Features Implemented**

### **Contact Management**
- 🗂️ View a list of contact emails  
- ➕ Add new contact emails with validation  
- ✏️ Edit existing contacts  
- ❌ Delete contacts with confirmation  
- 🚫 Prevent duplicate emails  
- ✅ Validate email format  

### **Employee Management**
- 👥 View and search employee list  
- ➕ Add/Edit employees with real-time validation  
- 🔄 Manage employee statuses 
- 🏖️ Track vacation dates  
- 🏗️ Assign employees to projects  
- 🚫 Prevent duplicate employee names  
- 🔍 Filter employees with search functionality  

---

## 📂 Project Structure

```bash
ProjectManagementApp/
├── Windows/            # Dialog window implementations
│   ├── ModernDialogWindow.cs     # Base dialog window
│   ├── ContactDialog.xaml        # Add/Edit contact dialog
│   ├── EmployeeDialog.xaml       # Add/Edit employee dialog
│   ├── EmployeeStatusDialog.xaml # Status management
│   ├── ConfirmationDialog.xaml   # Deletion confirmation
│   └── ErrorDialog.xaml          # Error messages
├── Pages/             # Main application pages
│   ├── ContactsPage.xaml         # Contact management
│   └── EmployeesPage.xaml        # Employee management
├── ViewModels/        # MVVM view models
│   ├── BaseViewModel.cs          # Base MVVM implementation
│   ├── ContactsViewModel.cs      # Contact management logic
│   └── EmployeesViewModel.cs     # Employee management logic
├── Models/            # Data models
│   ├── Contact.cs               # Contact entity
│   ├── Employee.cs              # Employee entity
│   ├── Project.cs               # Project entity
│   └── Equipment.cs             # Equipment entity
├── Repositories/      # Data access layer
│   ├── IContactRepository.cs    # Contact repository interface
│   ├── ContactRepository.cs     # Contact repository
│   ├── IEmployeeRepository.cs   # Employee repository interface
│   ├── EmployeeRepository.cs    # Employee repository
│   ├── IProjectRepository.cs    # Project repository interface
│   └── ProjectRepository.cs     # Project repository
├── Services/          # Business logic
│   ├── IProjectAssignmentService.cs
│   └── ProjectAssignmentService.cs
├── Styles/           # XAML styles
│   └── ModernStyles.xaml       # Shared styles
├── Validators/        # Input validation
│   └── EmailValidator.cs        # Email validation logic
├── Data/              # Database context
│   └── ApplicationDbContext.cs  # EF Core context
└── App.xaml           # Application configuration
```

---

## **📋 Database Structure**

### **Contacts Table**
| Column Name  | Data Type      | Constraints                  |
|--------------|----------------|------------------------------|
| `Id`         | INT            | Primary Key, Identity        |
| `Email`      | NVARCHAR(255)  | Unique                      |

### **Employees Table**
| Column Name     | Data Type      | Constraints                  |
|-----------------|----------------|------------------------------|
| `Id`            | INT            | Primary Key, Identity        |
| `FirstName`     | NVARCHAR(255)  | Required                     |
| `LastName`      | NVARCHAR(255)  | Required                     |
| `Status`        | NVARCHAR(50)   | Required                     |
| `ProjectId`     | INT            | Foreign Key, Nullable        |
| `VacationStart` | DATETIME       | Nullable                     |
| `VacationEnd`   | DATETIME       | Nullable                     |

### **Projects Table**
| Column Name | Data Type      | Constraints                  |
|-------------|----------------|------------------------------|
| `Id`        | INT            | Primary Key, Identity        |
| `Name`      | NVARCHAR(255)  | Required                     |

---

## **🔑 Key Components**

### **UI Components**
- 🖤 **Dark theme design** with #2D2D30  
- ❤️ **Red accent color** with #E6252D  
- 📋 Real-time validation feedback  
- 🎛️ Custom-styled controls (ComboBox, DatePicker, etc.)  
- 🔍 Search functionality  
- 💻 Mobile-friendly layouts  
- ✨ Modern dialog window style with draggable windows  

### **Validation System**
- ✉️ Email format validation  
- 🔒 Required field validation  
- 🚫 Duplicate entry prevention  
- ⚙️ Real-time validation with async checks  
- 🔄 Cross-field validation  
- 🛠️ Status-dependent validation  

### **Status Management**
- 🏢 Base status 
- 🏗️ Work status with project assignment  
- 🏖️ Vacation status with date tracking  

---

## **📦 Dependencies**

- `Microsoft.EntityFrameworkCore.SqlServer`  
- `Microsoft.EntityFrameworkCore.Tools`  
- `Microsoft.EntityFrameworkCore.Design`  
- `Microsoft.Extensions.Configuration.Json`  
- `Microsoft.Extensions.DependencyInjection`  

---

## **🧩 Design Patterns Used**

- **MVVM (Model-View-ViewModel)**  
- **Repository Pattern**  
- **Command Pattern**  
- **Dependency Injection**  
- **Observer Pattern** (via `INotifyPropertyChanged`)  
- **Factory Pattern** (for dialogs)  

---

## **🚀 Future Development**

Planned features include:  
- 🛠️ Equipment management system  
- 🏢 HQ resource overview  
- 📅 Project scheduling and management  
- 📊 Report generation and export  
- ✉️ Email integration for reports  
- 🔄 Resource allocation tracking  

---

## **📌 Project Status**

Contact and Employee management systems are fully implemented. Currently developing the **equipment management functionality**, with plans for **project management** and **reporting** features.