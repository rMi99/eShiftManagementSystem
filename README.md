# e-Shift Management System

This is a comprehensive desktop application for managing a household goods shifting business, built with .NET Core and Windows Forms. It provides a complete solution for handling customers, jobs, staff, vehicles, and financials.

-----

## Key Features

  * **Customer Management**: Add, edit, delete, and search for customers.
  * **Job Management**: Create, update, and track jobs from start to finish. You can also assign jobs to specific drivers, vehicles, and containers.
  * **Staff and Driver Management**: Manage staff information, including positions and contact details. You can also assign drivers to jobs and vehicles.
  * **Vehicle and Container Management**: Keep track of your fleet of vehicles and containers, including their availability and service history.
  * **Financials**: Manage payments and generate invoices for completed jobs.
  * **Reporting**: Generate various reports, such as job statistics, customer activity, and revenue analysis.
  * **User Authentication**: Secure login system with different roles and permissions for administrators, staff, and customers.
  * **Audit Logging**: Track all important actions performed within the system for security and accountability.

-----

## Installation Instructions

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/rmi99/eshiftmanagementsystem.git
    ```

2.  **Open in Visual Studio:**

      * Open the `eShiftManagementSystem.sln` file in Visual Studio 2022 or later.
      * Make sure you have the **.NET desktop development** workload installed.

3.  **Restore NuGet packages:**

      * Right-click on the solution in the Solution Explorer and select **Restore NuGet Packages**.

4.  **Set up the database:**

      * The connection string in `appsettings.json` is configured for a local MySQL server.
      * Make sure you have a MySQL server running and create a database named `db_eshift`.
      * You will need to create the necessary tables in the database. You can do this by running the SQL scripts that are not included in the project.

-----

## How to Run the Project

1.  **Set the startup project:**

      * In the Solution Explorer, right-click on the **eShiftManagementSystem** project and select **Set as Startup Project**.

2.  **Run the application:**

      * Press **F5** or click the **Start** button in Visual Studio to run the application.

-----

## Technologies Used

  * **.NET 8.0**
  * **Windows Forms**
  * **MySQL**
  * **MaterialSkin 2**
  * **QuestPDF**
  * **EPPlus**

-----

## Contribution Guidelines

Currently, we are not accepting external contributions. However, if you have any suggestions or find any bugs, please open an issue on the GitHub repository.

-----

## License

This project is not currently licensed.

-----

## Suggestions for Improvement

### Project Structure:

  * **Data Access Layer (DAL)**: You have a `Repositories` folder inside the `DataAccess` folder. It would be better to rename `DataAccess` to `DataAccessLayer` and have the `Repositories` folder directly inside it. This makes the project structure more intuitive and easier to navigate.
  * **Business Logic Layer (BLL)**: You have a `Business` folder with `Interfaces` and `Services` inside. This is a good structure, but you could rename `Business` to `BusinessLogicLayer` for clarity.
  * **User Interface (UI)**: The `Forms` and `Panels` are well-organized, but you could group them further by functionality. For example, you could have a `Customer` folder inside `Forms` that contains all customer-related forms and panels.

### Best Practices:

  * **Dependency Injection**: You are manually creating instances of your repositories and services in your forms. It would be better to use a dependency injection container like the one provided by .NET Core to manage the creation and lifetime of these objects. This would make your code more modular, testable, and maintainable.
  * **Error Handling**: You have some basic error handling, but you could improve it by using a centralized error logging mechanism. This would make it easier to track and debug errors in your application.
  * **Code Comments**: Your code is well-written, but you could add more comments to explain the purpose of your methods and classes. This would make it easier for other developers to understand your code.
  * **Unit Tests**: You don't have any unit tests in your project. It would be a good idea to add unit tests for your business logic and data access layers to ensure that your code is working as expected.

I hope this helps\!
