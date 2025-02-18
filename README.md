# Employee Admin Portal

This is a .NET 8 web API project for managing employees in an organization. It provides endpoints to perform CRUD operations on employee data.

## Technologies Used

- .NET 8
- C# 12.0
- Entity Framework Core

## Project Structure

- `Controllers/EmployeesController.cs`: Contains the API endpoints for managing employees.
- `Models/Entities/Employee.cs`: Defines the `Employee` entity.
- `Models/Entities/AddEmployeeDTO.cs`: Defines the data transfer object for adding a new employee.
- `Models/Entities/UpdateEmployeeDTO.cs`: Defines the data transfer object for updating an existing employee.
- `Data/ApplicationDbContext.cs`: Configures the database context for Entity Framework Core.
- `appsettings.json`: Contains configuration settings for the application.
- `Program.cs`: Configures and runs the application.

## Endpoints

### Get All Employees

- **URL**: `/api/employees`
- **Method**: `GET`
- **Description**: Retrieves a list of all employees.

### Get Employee by ID

- **URL**: `/api/employees/{id}`
- **Method**: `GET`
- **Description**: Retrieves an employee by their ID.

### Add New Employee

- **URL**: `/api/employees`
- **Method**: `POST`
- **Description**: Adds a new employee.
- **Request Body**:
```
{	
	"name": "string", 
	"email": "string", 
	"phone": "string", 
	"salary": "decimal" 
}
```
### Update Employee

- **URL**: `/api/employees/{id}`
- **Method**: `PUT`
- **Description**: Updates an existing employee.
- **Request Body**:
```
{	
	"name": "string", 
	"email": "string", 
	"phone": "string", 
	"salary": "decimal" 
}
```
### Delete Employee

- **URL**: `/api/employees/{id}`
- **Method**: `DELETE`
- **Description**: Deletes an employee by their ID.

## Setup Instructions

1. Clone the repository.
2. Open the solution in Visual Studio 2022.
3. Update the `appsettings.json` file with your database connection string.
4. Run the following commands in the Package Manager Console to apply migrations:
```
update-Database
```5. Build and run the application.

## License

This project is licensed under the MIT License.
