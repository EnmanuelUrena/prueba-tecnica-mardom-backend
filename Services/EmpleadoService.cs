using Prueba_tecnica_mardom_01.Models;
using Prueba_tecnica_mardom_01.Repositories;

namespace Prueba_tecnica_mardom_01.Services
{
    public interface IEmpleadoService
    {
        Task<List<Empleado>> GetEmployeesAsync();
        Task<bool> CreateEmployeeAsync(Empleado employee);
        Task<Empleado> GetEmployeeByDocumentAsync(string document);
        Task<List<Empleado>> GetEmployeeWithoutDuplicatesAsync();
        Task<List<Empleado>> GetEmployeeWithSalaryRangeAsync(int min, int max);
        Task<List<Empleado>> GetEmployeeWithSalaryIncreaseAsync();
        Task<object> GetGenderPercentageAsync();
        Task<bool> DeleteEmployeeAsync(string document);
    }

    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;

        public EmpleadoService(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public async Task<bool> CreateEmployeeAsync(Empleado employee)
        {
            var employees = await _empleadoRepository.GetEmployeesAsync();
            var findedEmployee = employees.FirstOrDefault(e => e.Document == employee.Document);
            if (findedEmployee != null)
            {
                return false;
            }
            return await _empleadoRepository.CreateEmployeeAsync(employee);
        }

        public async Task<List<Empleado>> GetEmployeesAsync()
        {
            return await _empleadoRepository.GetEmployeesAsync();
        }

        public async Task<Empleado> GetEmployeeByDocumentAsync(string document)
        {
            try
            {
                var employees = await _empleadoRepository.GetEmployeesAsync();
                var findedEmployee = employees.First(e => e.Document == document);
                return findedEmployee;
            }
            catch (Exception)
            {
                throw new Exception("Empleado no encontrado");
            }
        }

        public async Task<List<Empleado>> GetEmployeeWithoutDuplicatesAsync()
        {
            var employees = await _empleadoRepository.GetEmployeesAsync();
            employees = employees.GroupBy(e => new { e.Name, e.LastName }).Select(g => g.First()).ToList();
            return employees;
        }

        public async Task<List<Empleado>> GetEmployeeWithSalaryRangeAsync(int min, int max)
        {
            var employees = await _empleadoRepository.GetEmployeesAsync();
            var filteredEmployees = employees.Where(e => e.Salary >= min && e.Salary <= max).ToList();
            return filteredEmployees;
        }

        public async Task<List<Empleado>> GetEmployeeWithSalaryIncreaseAsync()
        {
            var employees = await _empleadoRepository.GetEmployeesAsync();
            var increasedEmployees = employees.Select(e =>
            {
                e.Salary += e.Salary >= 100000 ? e.Salary * 0.25 : e.Salary * 0.3;
                return e;
            }).ToList();

            return increasedEmployees;
        }

        public async Task<object> GetGenderPercentageAsync()
        {
            var employees = await _empleadoRepository.GetEmployeesAsync();
            var totalEmployees = employees.Count;
            var maleEmployees = Math.Round((double)employees.Count(e => e.Gender == 'M') / totalEmployees * 100, 2);
            var femaleEmployees = Math.Round((double)employees.Count(e => e.Gender == 'F') / totalEmployees * 100, 2);

            return new { maleEmployees = $"{maleEmployees:0.##}%", femaleEmployees = $"{femaleEmployees:0.##}%" }; ;
        }

        public async Task<bool> DeleteEmployeeAsync(string document)
        {
            return await _empleadoRepository.DeleteEmployeeAsync(document);
        }
    }
}
