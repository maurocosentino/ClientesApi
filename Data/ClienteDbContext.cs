using System;
using ClienteApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClienteApi.Data;

public class ClienteDbContext : DbContext
{
    public ClienteDbContext(DbContextOptions<ClienteDbContext> option) : base(option)
    {

    }
    
    public DbSet<Cliente> Clientes { get; set; } 
}
