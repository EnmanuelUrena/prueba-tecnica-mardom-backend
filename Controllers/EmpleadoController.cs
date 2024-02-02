using Microsoft.AspNetCore.Mvc;
using Prueba_tecnica_mardom_01.Models;
using System.Text.Json;


namespace Prueba_tecnica_mardom_01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetAll()
        {
            var textFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data.txt");

            if (!System.IO.File.Exists(textFilePath))
            {
                return NotFound("No se encontró el archivo de empleados");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);
            var empleados = JsonSerializer.Deserialize<List<Empleado>>(jsonData);


            return Ok(empleados);
        }

        [HttpGet]
        [Route("SinDuplicados")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetWithOutDuplicates()
        {
            var textFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data.txt");

            if (!System.IO.File.Exists(textFilePath))
            {
                return NotFound("No se encontró el archivo de empleados");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(textFilePath);
            var empleados = JsonSerializer.Deserialize<List<Empleado>>(jsonData);

            empleados = empleados.GroupBy(e => new { e.Name, e.LastName }).Select(g => g.First()).ToList();

            return Ok(empleados);
        }

    }
}
