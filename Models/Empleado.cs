namespace Prueba_tecnica_mardom_01.Models
{

    public class Empleado
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Document { get; set; }
        public double Salary { get; set; }
        public char Gender { get; set; }
        public required string Position { get; set; }
        public DateOnly StartDate { get; set; }

    }
}
