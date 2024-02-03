using Microsoft.AspNetCore.Mvc;
using Prueba_tecnica_mardom_01.Models;
using Prueba_tecnica_mardom_01.Services;


namespace Prueba_tecnica_mardom_01.Controllers
{
    /// <summary>
    /// This class is the controller for the employee
    /// </summary>
    [ApiController]
    [Route("api/employee")]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoService _employeeService;

        /// <summary>
        /// This is the constructor of the class
        /// </summary>
        /// <param name="employeeService"></param>
        public EmpleadoController(IEmpleadoService employeeService)
        {
            _employeeService = employeeService;
        }

        private static List<Empleado> employee = new List<Empleado>();

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>The collection of employees</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Empleado>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmployee()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            return Ok(employees);
        }
        /// <summary>
        /// Get employee by document
        /// </summary>
        /// <param name="document"></param>
        /// <returns>A employee that matches with the document</returns>
        [HttpGet]
        [Route("find-by-document")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Empleado))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Empleado>> GetEmployeeFindByDocument(string document)
        {
            try
            {
                var findedEmployee = await _employeeService.GetEmployeeByDocumentAsync(document);
                return Ok(findedEmployee);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        /// <summary>
        /// Get employees without duplicates
        /// </summary>
        /// <returns>The collection of employees without duplicates</returns>
        [HttpGet]
        [Route("without-duplicates")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Empleado>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmployeeWithoutDuplicates()
        {
            var employees = await _employeeService.GetEmployeeWithoutDuplicatesAsync();
            return Ok(employees);
        }

        /// <summary>
        /// Get employees with salary range
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>The collection of employees between min and max</returns>
        [HttpGet]
        [Route("salary-range")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Empleado>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmployeeWithSalaryRange(int min = 0, int max = 100000)
        {
            var filteredEmployees = await _employeeService.GetEmployeeWithSalaryRangeAsync(min, max);
            return Ok(filteredEmployees);
        }
        /// <summary>
        /// Get employees with salary increase
        /// </summary>
        /// <returns>The collection of employees with an Salary Increase</returns>
        [HttpGet]
        [Route("salary-increase")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Empleado>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmployeeWithSalaryIncrease()
        {
            var increasedEmployees = await _employeeService.GetEmployeeWithSalaryIncreaseAsync();
            return Ok(increasedEmployees);
        }
        /// <summary>
        /// Get gender percentage of all employees
        /// </summary>
        /// <returns>An object with percentages of males and females</returns>
        [HttpGet]
        [Route("gender-percentage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetGenderPercentage()
        {
            var genderPercentage = await _employeeService.GetGenderPercentageAsync();
            return Ok(genderPercentage);
        }
        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="newEmployee"></param>
        /// <returns>The new employee</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Empleado))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Empleado>> CreateEmployee(Empleado newEmployee)
        {

            //TODO
            if (newEmployee == null)
            {
                return BadRequest("Datos del empleado proporcionados incorrectamente");
            }

            var created = await _employeeService.CreateEmployeeAsync(newEmployee);

            if (created == false)
            {
                return BadRequest("El empleado ya existe");
            }

            return CreatedAtAction(nameof(GetEmployeeFindByDocument), new { document = newEmployee.Document }, newEmployee);
        }
        /// <summary>
        ///  Delete an employee
        /// </summary>
        /// <param name="document"></param>
        /// <returns>No content if it deleted</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEmployee(string document)
        {
            var removed = await _employeeService.DeleteEmployeeAsync(document);

            if (removed == false)
            {
                return NotFound("Documento no existente");
            }

            return NoContent();

        }

    }
}
