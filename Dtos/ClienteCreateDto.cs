using System;
using System.ComponentModel.DataAnnotations;

namespace ClienteApi.Dtos;

public class ClienteCreateDto
{
    [Required]
    [MaxLength(30)]
    public string Nombre { get; set; } = "";

    [Required]
    [MaxLength(30)]
    public string Apellido { get; set; } = "";
    
    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; } = "";

     [Required]
    public string Telefono { get; set; } = "";
    
     [Required]
    [MaxLength(30)]
    public string Direccion { get; set; } = "";
}
