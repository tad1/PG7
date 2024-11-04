using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DomainContext : DbContext, IApplicationDbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<Employment> Employments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(builder =>
            {
                builder.HasOne(p => p.Father)
                    .WithMany();
                
                builder.HasOne(p => p.Mother)
                    .WithMany();

                builder.HasOne(p => p.Spouse)
                    .WithOne();

                builder.HasMany(p => p.Siblings)
                    .WithMany();
                
                builder.OwnsMany(p => p.PhoneNumbers);

                builder.HasDiscriminator<string>("type")
                    .HasValue<Person>("person_base")
                    .HasValue<PublicPerson>("public_person");
                
            }
        );

        modelBuilder.Entity<Employment>(builder =>
        {
            builder.HasOne(p => p.Person)
                .WithMany(p => p.Employments)
                .OnDelete(DeleteBehavior.Cascade);
            builder.OwnsOne(p => p.EmploymentType);
        });
        
        modelBuilder.Owned<PhoneNumber>();
        modelBuilder.Owned<EmploymentType>();
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseNpgsql("Host=localhost; Database=people; Username=postgres; Password=secret");
        base.OnConfiguring(optionsBuilder);
    }
}