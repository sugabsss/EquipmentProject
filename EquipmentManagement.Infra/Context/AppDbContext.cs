using Microsoft.EntityFrameworkCore;
using EquipmentManagement.Domain.Entities;

namespace EquipmentManagement.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Equipment> Equipaments { get; set; }
    }
}

