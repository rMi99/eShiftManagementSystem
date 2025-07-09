# e-Shift Transport Management System

## Overview

This is a comprehensive Windows Forms application built with C# (.NET 8) and MaterialSkin2 UI framework for managing transport operations for household goods and logistics companies. The system provides complete automation for daily operations including job management, resource allocation, scheduling, and audit tracking.

## Features

### 🔐 Authentication & Security
- Multi-role authentication (Super Admin, Admin, Customer, Driver, Staff)
- Secure password hashing with BCrypt
- Role-based access control (RBAC)
- Session management and timeout controls
- Comprehensive audit logging

### 📊 Core Management Modules
- **Job Management**: Complete job lifecycle from request to completion
- **Load Management**: Multiple loads per job with product tracking
- **Quote System**: Generate, approve/decline transport quotes
- **Vehicle Management**: Fleet management with maintenance tracking
- **Staff Management**: Drivers, assistants, and administrative staff
- **Container Management**: Container allocation and tracking
- **Transport Unit Management**: Combine vehicles, drivers, assistants, and containers
- **Customer Management**: Complete customer relationship management

### 📈 Business Intelligence
- **Audit Trail**: Complete audit logging for all critical operations
- **Real-time Status Tracking**: Job and resource status updates
- **Report Generation**: Comprehensive reporting system
- **Resource Assignment**: Automatic and manual resource allocation

### 🎨 User Interface
- Modern Material Design with MaterialSkin2
- Responsive dashboard layouts
- Tabbed navigation for different modules
- Consistent theming and styling
- User-friendly data grids and forms

## Architecture

### Clean Architecture Implementation
```
├── Presentation Layer (UI)
│   ├── Forms/               # Windows Forms
│   ├── Panels/             # Management panels
│   └── Controls/           # Custom controls
├── Business Logic Layer (BLL)
│   ├── Services/           # Business services
│   └── Interfaces/         # Service contracts
├── Data Access Layer (DAL)
│   ├── Repositories/       # Data repositories
│   └── Interfaces/         # Repository contracts
├── Models/                 # Domain entities
├── Utils/                  # Utility classes
└── Database/               # Database schema
```

## Database Schema

### Core Tables
- **users**: User authentication and roles
- **customers**: Customer information and registration
- **jobs**: Transport job requests and management
- **job_status_history**: Job status change tracking
- **loads**: Individual loads within jobs
- **load_products**: Products within loads (many-to-many)
- **quotes**: Transport cost quotations
- **vehicles**: Fleet vehicle management
- **vehicle_types**: Vehicle type categories
- **drivers**: Driver-specific information
- **assistants**: Assistant staff information
- **staff**: General staff information
- **containers**: Container inventory
- **transport_units**: Combined resource units
- **products**: Product catalog
- **product_categories**: Product categorization
- **audit_logs**: Comprehensive audit trail

## Technology Stack

- **Framework**: .NET 8 Windows Forms
- **UI**: MaterialSkin2 for Material Design
- **Database**: MySQL/MariaDB
- **Data Access**: ADO.NET with MySQL.Data
- **Architecture**: Clean Architecture with Repository Pattern
- **Security**: BCrypt password hashing
- **Logging**: Serilog integration
- **Configuration**: JSON configuration management

## Key Components

### 1. Models
All domain entities with computed properties and navigation relationships:
- User, Customer, Job, Load, Quote, Vehicle, Staff, Driver, Assistant, Container, TransportUnit, Product, AuditLog

### 2. Repositories
Complete data access layer with CRUD operations:
- UserRepository, CustomerRepository, JobRepository, LoadRepository, QuoteRepository
- VehicleRepository, DriverRepository, AssistantRepository, ContainerRepository
- TransportUnitRepository, ProductRepository, AuditLogRepository

### 3. Business Services
- **AuditService**: Comprehensive audit logging for all operations
- **JobAssignmentService**: Automatic and manual resource allocation
- **AuthenticationService**: User authentication and session management

### 4. UI Panels
Professional Material Design management panels:
- **ContainerManagementPanel**: Full container CRUD operations
- **DriverManagementPanel**: Driver management with license tracking
- **TransportUnitManagementPanel**: Resource combination management
- **AuditLogViewerPanel**: Audit trail viewing with filters and pagination
- Plus existing panels for jobs, customers, vehicles, users, and quotes

## Security Features

- **Password Security**: BCrypt hashing with salt
- **SQL Injection Prevention**: Parameterized queries throughout
- **Input Validation**: Comprehensive form validation
- **Session Management**: Secure session handling
- **Audit Logging**: Complete action tracking with IP addresses
- **Role-Based Access**: Different access levels for different user types

## Installation & Setup

### Prerequisites
- Windows 10/11
- .NET 8 Runtime
- MySQL Server 8.0+ or MariaDB 10.6+
- Visual Studio 2022 (for development)

### Database Setup
1. Install MySQL/MariaDB
2. Run the database schema script: `Database/schema.sql`
3. Update connection string in `appsettings.json`

### Application Setup
1. Clone the repository
2. Open `eShiftManagementSystem.sln` in Visual Studio 2022
3. Restore NuGet packages
4. Update database connection string
5. Build and run the application

## Configuration

### Database Connection
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=eshift_db;Uid=your_user;Pwd=your_password;SslMode=None;"
  }
}
```

### Application Settings
```json
{
  "Application": {
    "Name": "e-Shift Management System",
    "Version": "1.0.0",
    "Company": "Your Company Name"
  },
  "Security": {
    "SessionTimeoutMinutes": 30,
    "MaxLoginAttempts": 5,
    "LockoutDurationMinutes": 15
  }
}
```

## Usage

### User Roles
- **Super Admin**: Full system access, user management
- **Admin**: Management operations, report generation
- **Customer**: Job requests, status tracking
- **Driver**: Job assignments, status updates
- **Staff**: Operational tasks

### Main Features
1. **Job Management**: Create, assign, and track transport jobs
2. **Resource Management**: Manage vehicles, drivers, and equipment
3. **Quote System**: Generate and manage transport quotations
4. **Audit Trail**: Track all system changes and user actions
5. **Reporting**: Generate operational and management reports

## Development

### Adding New Features
1. Create domain models in `Models/`
2. Implement repositories in `DataAccess/Repositories/`
3. Add business logic in `Business/Services/`
4. Create UI forms/panels in `Forms/` or `Forms/Panels/`
5. Update database schema if needed

### Code Standards
- Follow Clean Architecture principles
- Use Repository pattern for data access
- Implement comprehensive error handling
- Add audit logging for critical operations
- Follow Material Design guidelines for UI

## Contributing

1. Fork the repository
2. Create a feature branch
3. Follow the established architecture patterns
4. Add appropriate tests
5. Update documentation
6. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support and questions:
- Create an issue in the GitHub repository
- Check the documentation in the `docs/` folder
- Review the code comments for implementation details

## Version History

### v1.0.0 (Current)
- Complete transport management system
- Full CRUD operations for all entities
- Comprehensive audit logging
- Material Design UI
- Role-based access control
- Database schema with sample data
- Production-ready with error handling

---

**e-Shift Management System** - Streamlining transport operations with modern technology.