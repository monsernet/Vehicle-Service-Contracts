
# Automated Vehicle Service Contract Follow-up System

This repository hosts the source code for an automated vehicle service contract follow-up system built on the MVC (Model-View-Controller) framework. Leveraging the repository pattern and DTOs (Data Transfer Objects), this system efficiently manages vehicle service contracts and associated tasks while ensuring clean separation of concerns and data integrity.
## Key Features:
- **MVC Architecture:** Utilizes the MVC design pattern for structured and modular development.
- **Repository Pattern:** Implements the repository pattern for data access and management, promoting code reusability and testability.
- **Service WSDL Integration:** Seamlessly extracts data from external service WSDL to retrieve vehicle and customer information.
- **Dynamic Contract Generation:** Automatically defines contract details for selected vehicles based on mileage and optional services.
- **Comprehensive Management Modules:**
    - Vehicle Brand and Type Management
    - Service Mileage Cost Management
    - Vehicle Service History Tracking
    - Vehicle History Management
    - Genuine and Additional Parts Management
    - Service Packages Cost Management
    - User Management with Admin and Service Advisers roles
- **Quotation Management:** Generates service quotations for customer approval before contract finalization.
- **Automated Package Proposal:** Suggests the best service package based on past services, required part replacements, and cost prediction.
- **Reporting Capabilities:** 
    - General Reports: Daily, weekly, and monthly statistics of contracts, quotations, and efficiency rates.
    - User Reports: Daily, weekly, and monthly performance reports to track user productivity.
## Technologies Used:
- ASP.NET Core 7
- MVC framework
- Entity Framework core
- RestFull API
- SQL Server
- Dependency Injection
- Asynchronous Programming Patterns
- DTO (Data Transfer Objects)
- PDFservice
- Excel Service
## 
This system combines the power of MVC architecture, repository pattern, and DTOs to deliver a robust and efficient solution for managing vehicle service contracts. With seamless integration of external services and comprehensive management modules, it offers a user-friendly interface and reliable performance, ensuring streamlined operation and enhanced productivity.


