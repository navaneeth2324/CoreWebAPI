using System.Text.Json;
using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesController> logger;
        private readonly IMemoryCache _cache;

        public EmployeesController(ApplicationDbContext context,ILogger<EmployeesController> logger,IMemoryCache cache)
        {
            _context = context;
            this.logger = logger;
            _cache = cache;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            logger.LogInformation("Fetching all employees");

            const string cacheKey = "all_employees";
            if (!_cache.TryGetValue(cacheKey, out List<Employee> employees))
            {
                employees = _context.Employees.ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60)) // Cache for 10 Seconds
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Cache for 5 minutes

                _cache.Set(cacheKey, employees, cacheEntryOptions);

                logger.LogInformation("Employees fetched from database and cached.");
            }
            else
            {
                logger.LogInformation("Employees fetched from cache.");
            }

            logger.LogInformation($"Employees fetched: {JsonSerializer.Serialize(employees)}");
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
            _cache.Remove("all_employees"); // Invalidate the cache
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
            _cache.Remove("all_employees"); // Invalidate the cache
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
            _cache.Remove("all_employees"); // Invalidate the cache
            logger.LogInformation("Employee Deleted");
            return Ok();
        }
    }
}
