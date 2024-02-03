using Prueba_tecnica_mardom_01.Models;
using System.Text.Json;

namespace Prueba_tecnica_mardom_01.Repositories
{
    public interface IEmpleadoRepository
    {
        Task<List<Empleado>> GetEmployeesAsync();
        Task<bool> CreateEmployeeAsync(Empleado empleado);
        Task<bool> DeleteEmployeeAsync(string document);
    }

    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly string textFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/data.txt");

        public async Task<List<Empleado>> GetEmployeesAsync()
        {
            try
            {
                if (!System.IO.File.Exists(textFilePath))
                {
                    throw new Exception("No se encontró el archivo de empleados");
                }

                //Leemos el archivo y lo almacenamos en una variable
                var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);

                //Creamos una lista empleados donde se guardara el JSON una vez deserializado
                List<Empleado>? employee = JsonSerializer.Deserialize<List<Empleado>>(jsonData);

                return employee ?? [];

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateEmployeeAsync(Empleado employee)
        {
            try
            {
                if (!System.IO.File.Exists(textFilePath))
                {
                    throw new Exception("No se encontró el archivo de empleados");
                }

                var jsonEmployee = JsonSerializer.Serialize(employee);
                await System.IO.File.WriteAllTextAsync(textFilePath, jsonEmployee);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteEmployeeAsync(string document)
        {
            try
            {
                var employee = await GetEmployeesAsync();

                var employeeToRemove = employee.Find(e => e.Document == document);

                if (employeeToRemove == null)
                {
                    return false;
                }
                employee.Remove(employeeToRemove);

                var jsonEmployee = JsonSerializer.Serialize(employee);
                await System.IO.File.WriteAllTextAsync(textFilePath, jsonEmployee);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
