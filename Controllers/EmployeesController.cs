using System.Text.Json;
using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesController> logger;

        public EmployeesController(ApplicationDbContext context,ILogger<EmployeesController> logger)
        {
            _context = context;
            this.logger = logger;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            logger.LogInformation("Getting all employees");
            var employees=_context.Employees.ToList();
            logger.LogInformation($"Employees fetched: {JsonSerializer.Serialize(employees)}");
            //Adding this exception to test the ExceptionHandlerMiddleware
            throw new Exception("Some Error Occured");
            return Ok(employees);
        }
        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDTO addEmployeeDTO)
        {
            logger.LogInformation("Adding new Employee");
            var employee = new Employee
            {
                Name = addEmployeeDTO.Name,
                Email = addEmployeeDTO.Email,
                Phone = addEmployeeDTO.Phone,
                Salary = addEmployeeDTO.Salary
            };
            _context.Employees.Add(employee);
            _context.SaveChanges();
            logger.LogInformation($"Added New Employee {JsonSerializer.Serialize(employee)}");
            return Ok(employee); 
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployee(Guid id)
        {
            logger.LogInformation("Fetching Employee by ID");
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            logger.LogInformation($"Employee Fetched {JsonSerializer.Serialize(employee)}");
            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDTO updateEmployeeDTO)
        {
            logger.LogInformation("Updating Employee Details ");
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Name = updateEmployeeDTO.Name;
            employee.Email = updateEmployeeDTO.Email;
            employee.Phone = updateEmployeeDTO.Phone;
            employee.Salary = updateEmployeeDTO.Salary;
            _context.SaveChanges();
            logger.LogInformation($"Updated Employee Details {JsonSerializer.Serialize(employee)}");
            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            logger.LogInformation("Deleting Employee");
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            logger.LogInformation("Employee Deleted");
            return Ok();
        }
    }
}
