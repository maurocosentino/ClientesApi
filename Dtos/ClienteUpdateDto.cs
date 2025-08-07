using System.ComponentModel.DataAnnotations;

namespace ClienteApi.Dtos
{
    public class ClienteUpdateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser mayor que 0")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(30, ErrorMessage = "El nombre no debe superar los 30 caracteres")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(30, ErrorMessage = "El apellido no debe superar los 30 caracteres")]
        public string Apellido { get; set; } = "";

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [MaxLength(50, ErrorMessage = "El email no debe superar los 50 caracteres")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string Telefono { get; set; } = "";

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [MaxLength(100, ErrorMessage = "La dirección no debe superar los 100 caracteres")]
        public string Direccion { get; set; } = "";
    }
}
