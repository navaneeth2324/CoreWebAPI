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
            return Ok(employees);
        }
        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDTO addEmployeeDTO)
        {
            var employee = new Employee
            {
                Name = addEmployeeDTO.Name,
                Email = addEmployeeDTO.Email,
                Phone = addEmployeeDTO.Phone,
                Salary = addEmployeeDTO.Salary
            };
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return Ok(employee); 
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployee(Guid id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDTO updateEmployeeDTO)
        {
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
            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return Ok();
        }
    }
}
