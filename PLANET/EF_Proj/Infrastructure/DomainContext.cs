using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DomainContext : DbContext, IApplicationDbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<Employment> Employments { get; set; }
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(builder =>
            {
                builder.HasOne(p => p.Father)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                
                builder.HasOne(p => p.Mother)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                
                builder.HasOne(p => p.Spouse)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                builder.OwnsMany(p => p.PhoneNumbers);
                builder.OwnsMany(p => p.Addresses);
            }
        );

        modelBuilder.Owned<Address>();
        modelBuilder.Owned<PhoneNumber>();
        modelBuilder.Owned<EmploymentType>();
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql("Host=localhost; Database=people; Username=postgres; Password=secret");
}