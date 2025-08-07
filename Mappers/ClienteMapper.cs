using System;
using ClienteApi.Dtos;
using ClienteApi.Models;

namespace ClienteApi.Mappers;

public static class ClienteMapper
{
    public static ClienteDto MapToDto(Cliente registro) => new()
    {
        Id = registro.Id,
        Nombre = registro.Nombre,
        Apellido = registro.Apellido,
        Email = registro.Email,
        Telefono = registro.Telefono,
        Direccion = registro.Direccion,
        FechaRegistro = registro.FechaRegistro
    };

    public static Cliente MapToModel(ClienteCreateDto dto) => new()
    {
        Nombre = dto.Nombre,
        Apellido = dto.Apellido,
        Email = dto.Email,
        Telefono = dto.Telefono,
        Direccion = dto.Direccion
    };

    public static Cliente MapToModel(ClienteUpdateDto dto, Cliente cliente)
    {
        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.Email = dto.Email;
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;

        return cliente;
    }
}
