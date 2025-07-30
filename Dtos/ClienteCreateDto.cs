using System;
using System.ComponentModel.DataAnnotations;

namespace ClienteApi.Dtos;

public class ClienteCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(30)]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [MaxLength(30)]
    public string Apellido { get; set; } = "";
    
    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; } = "";

    [Required]
    [Phone]
    public string Telefono { get; set; } = "";
    
    [Required]
    [MaxLength(100)]
    public string Direccion { get; set; } = "";
}
