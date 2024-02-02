using Microsoft.AspNetCore.Mvc;
using Prueba_tecnica_mardom_01.Models;
using System.Text.Json;


namespace Prueba_tecnica_mardom_01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        //Creamos el Path para el archivo txt que contiene la data
        private readonly string textFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data.txt");

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleado()
        {

            if (!System.IO.File.Exists(textFilePath))
            {
                //Retornamos un NotFound en caso de que el archivo no se encuentre
                return NotFound("No se encontró el archivo de empleados");
            }

            //Leemos el archivo y lo almacenamos en una variable
            var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);

            //Creamos una lista empleados donde se guardara el JSON una vez deserializado
            List<Empleado>? empleados = JsonSerializer.Deserialize<List<Empleado>>(jsonData);

            //Retornamos un json de empleados
            return Ok(empleados);
        }

        [HttpGet]
        [Route("SinDuplicados")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleadoWithoutDuplicates()
        {
            if (!System.IO.File.Exists(textFilePath))
            {
                return NotFound("No se encontró el archivo de empleados");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);
            List<Empleado>? empleados = JsonSerializer.Deserialize<List<Empleado>>(jsonData);

            //Agrupamos todos los empleados por nombre y apellido y seleccionamos el primero de cada grupo y lo convertimos a una lista
            empleados = empleados?.GroupBy(e => new { e.Name, e.LastName }).Select(g => g.First()).ToList();

            return Ok(empleados);
        }

        [HttpGet]
        [Route("RangoSalarial")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleadoWithSalaryRange(int min = 0, int max = 100000)
        {
            if (!System.IO.File.Exists(textFilePath))
            {
                return NotFound("No se encontró el archivo de empleados");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);
            List<Empleado>? empleados = JsonSerializer.Deserialize<List<Empleado>>(jsonData);

            var empleadosFiltrados = empleados?.Where(e => e.Salary >= min && e.Salary <= max);

            return Ok(empleadosFiltrados);
        }

        [HttpGet]
        [Route("AumentoSalarial")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleadoWithSalaryIncrease()
        { 
            if (!System.IO.File.Exists(textFilePath))
            {
                return NotFound("No se encontró el archivo de empleados");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);
            List<Empleado>? empleados = JsonSerializer.Deserialize<List<Empleado>>(jsonData);

            var empleadosAumentados = empleados?.Select(e => {
                if(e.Salary >= 100000){
                    e.Salary += e.Salary * 0.25;
                }
                if(e.Salary <= 100000){
                    e.Salary += e.Salary * 0.3;
                }
                return e;
            });
            return Ok(empleadosAumentados);
        }

        [HttpGet]
        [Route("PorcentajeGenero")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetGenderPercentaje()
        {
            if (!System.IO.File.Exists(textFilePath))
            {
                return NotFound("No se encontró el archivo de empleados");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);
            List<Empleado>? empleados = JsonSerializer.Deserialize<List<Empleado>>(jsonData);

            var male = empleados?.Count(e => e.Gender == 'M');
            var female = empleados?.Count(e => e.Gender == 'F');

            return Ok(new { male, female });
        }

        [HttpPost]
        public async Task<ActionResult<Empleado>> CreateEmpleado(Empleado newEmpleado)
        {
            if (newEmpleado == null)
            {
                return BadRequest("Datos del empleado proporcionados incorrectamente");
            }

            List<Empleado>? empleados = new List<Empleado>();

            if (System.IO.File.Exists(textFilePath))
            {
                var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);
                empleados = JsonSerializer.Deserialize<List<Empleado>>(jsonData);
            }
            if (empleados != null && empleados.Any(e => e.Name == newEmpleado.Name && e.LastName == newEmpleado.LastName))
            {
                return BadRequest("Ya existe un empleado con el mismo nombre y apellidos");
            }

            empleados ??= new List<Empleado>(); // Initialize empleados if it is null

            empleados.Add(newEmpleado);

            var jsonEmpleado = JsonSerializer.Serialize(empleados);
            await System.IO.File.WriteAllTextAsync(textFilePath, jsonEmpleado);

            return CreatedAtAction("Get", new { id = newEmpleado.Document }, newEmpleado);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmpleado(string document)
        {
            if (document == null)
            {
                return BadRequest("Falta proporcionar el documento");
            }

            List<Empleado>? empleados = new List<Empleado>();

            if (!System.IO.File.Exists(textFilePath))
            {
                return NotFound("No se encontró el archivo de empleados");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);
            empleados = JsonSerializer.Deserialize<List<Empleado>>(jsonData);

            var empleadoToRemove = empleados?.Find(e => e.Document == document);
            
            if (empleadoToRemove == null)
            {
                return NotFound("Documento no existente");
            }
            empleados?.Remove(empleadoToRemove);
            var jsonEmpleado = JsonSerializer.Serialize(empleados);
            await System.IO.File.WriteAllTextAsync(textFilePath, jsonEmpleado);
            
            return NoContent();
            
        }

    }
}
