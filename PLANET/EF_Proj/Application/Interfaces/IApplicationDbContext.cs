using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Person> People { get; set; }
    DbSet<Employment> Employments { get; set; }
    
    public int SaveChanges(); 
}