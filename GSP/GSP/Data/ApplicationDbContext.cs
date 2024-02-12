using AuthSystem.Areas.Identity.Data;
using GSP.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GSP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Entities
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Mileage> Mileages { get; set; }
        public DbSet<VehicleService> VehicleServices { get; set; }
        public DbSet<GeniumPart> GeniumParts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ServiceContract> ServiceContracts { get; set; }
        public DbSet<ServiceContractPart> ServiceContractsParts { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<ServiceContractAdditionalPart> ScAdditionalParts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeniumPart>()
                .Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<VehicleService>()
                .Property(x => x.Cost)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ServiceContractPart>()
                .Property(x => x.UnitCost)
                .HasColumnType("decimal(18,2)");

            //Relations between Vehicles and VehicleTypes
            modelBuilder.Entity<VehicleType>()
                .HasOne(vt => vt.Vehicle)
                .WithMany(v => v.VehicleTypes)
                .HasForeignKey(vt => vt.VehicleId);


            // Other configurations

            base.OnModelCreating(modelBuilder);
        }
    }
}