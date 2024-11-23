# Project Management Application

A **WPF desktop application** designed for project management, built using the **MVVM architectural pattern** with **Entity Framework Core** for database operations. The application features a robust **contact management system** with email validation and database persistence.

---

## 🛠️ Technical Stack

- **Framework**: .NET Core / WPF  
- **Architecture**: MVVM  
- **Database**: Microsoft SQL Server  
- **ORM**: Entity Framework Core  
- **Design Pattern**: Repository Pattern  
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection  

---

## ✨ Features Implemented

### **Contact Management**
- View a list of contact emails  
- Add new contact emails with validation  
- Edit existing contacts  
- Delete contacts with confirmation  
- Prevent duplicate emails  
- Validate email format  

---

## 📂 Project Structure

```
ProjectManagementApp/
├── Windows/            # Dialog window implementations
│   ├── ContactDialog.xaml        # Add/Edit contact dialog
│   ├── ConfirmationDialog.xaml   # Deletion confirmation
│   └── ErrorDialog.xaml          # Error messages
├── Pages/             # Main application pages
│   └── ContactsPage.xaml         # Contact management view
├── ViewModels/        # MVVM view models
│   ├── BaseViewModel.cs          # Base MVVM implementation
│   └── ContactsViewModel.cs      # Contact management logic
├── Models/            # Data models
│   └── Contact.cs               # Contact entity
├── Repositories/      # Data access layer
│   ├── IContactRepository.cs    # Repository interface
│   └── ContactRepository.cs     # Repository implementation
├── Validators/        # Input validation
│   └── EmailValidator.cs        # Email validation logic
├── Data/              # Database context
│   └── ApplicationDbContext.cs  # EF Core context
└── App.xaml           # Application configuration
```

## 📋 Database Structure

The application uses **SQL Server** with a `Contact` table containing the following columns:

| Column Name | Data Type       | Constraints              |
|-------------|-----------------|--------------------------|
| `Id`        | INT             | Primary Key, Identity    |
| `Email`     | NVARCHAR(255)   | Unique                  |

---

## 🔑 Key Components

### **Models**
- The `Contact` model represents email contacts with unique identifiers.

### **Database Context**
- `ApplicationDbContext` extends `DbContext` and manages `Contact` entities.

### **Repository Pattern Implementation**
The `IContactRepository` interface defines:  
- `GetAllContactsAsync`  
- `AddContactAsync`  
- `UpdateContactAsync`  
- `DeleteContactAsync`  

### **Validation**
- `EmailValidator` includes:
  - Email format validation using regex
  - Null or whitespace checking
  - Case-insensitive validation  

### **Dependency Injection Setup**
Configured services include:  
- `DbContext`  
- `Contact Repository`  
- `ViewModels`  

---

## 🌟 Key Functionalities

### **Email Validation**
- Format validation using regex  
- Duplicate checking against the database  
- Real-time validation feedback  
- Prevention of invalid data entry  

### **Database Operations**
- Asynchronous CRUD operations  
- Repository pattern for data access  
- Optimistic concurrency handling  
- Data persistence using Entity Framework Core  

### **MVVM Implementation**
- Property change notifications  
- Command pattern for user actions  
- View-ViewModel separation  
- Data binding  

---

## 📦 Dependencies

- `Microsoft.EntityFrameworkCore.SqlServer`  
- `Microsoft.EntityFrameworkCore.Tools`  
- `Microsoft.Extensions.Configuration.Json`  
- `Microsoft.Extensions.DependencyInjection`  

---

## ⚙️ Configuration

The application requires a connection string in the `appsettings.json` file for database connectivity.

---

## 🧩 Design Patterns Used

- **MVVM (Model-View-ViewModel)**  
- **Repository Pattern**  
- **Command Pattern**  
- **Dependency Injection**  
- **Observer Pattern** (via `INotifyPropertyChanged`)  

---

## 🚀 Future Development Areas

- Additional entity management  
- Extended data models  
- Reporting functionality  
- Data export capabilities  

---

## 📌 Project Status

The basic contact management functionality has been implemented. Plans are in place for expansion into a full project management system.
