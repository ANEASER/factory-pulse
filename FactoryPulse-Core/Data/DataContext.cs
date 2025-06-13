using Microsoft.EntityFrameworkCore;

namespace FactoryPulse_Core.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<Entities.Machine> Machines { get; set; } = null!;
    public DbSet<Entities.Inventory> Inventories { get; set; } = null!;
    public DbSet<Entities.SensorData> SensorData { get; set; } = null!;
    public DbSet<Entities.WorkOrder> WorkOrders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Machine>()
            .HasMany(m => m.SensorData)
            .WithOne(sd => sd.Machine)
            .HasForeignKey(sd => sd.MachineID);

        modelBuilder.Entity<Entities.Machine>()
            .HasMany(m => m.WorkOrders)
            .WithOne(wo => wo.Machine)
            .HasForeignKey(wo => wo.MachineID);

        modelBuilder.Entity<Entities.Inventory>()
            .Property(i => i.Barcode)
            .IsRequired();

        modelBuilder.Entity<Entities.SensorData>()
            .Property(sd => sd.Temperature)
            .IsRequired();

        modelBuilder.Entity<Entities.WorkOrder>()
            .Property(wo => wo.IssueType)
            .IsRequired();

    }
}