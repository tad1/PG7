using Application.Interfaces;
using Domain;

namespace Application;

// CreateEmploymentType
// RemoveEmploymentType

public class Employments
{
    IApplicationDbContext _context;

    public Employments(IApplicationDbContext context)
    {
        _context = context;
    }
    
    // CREATE
    public record AddEmploymentCommand
    {
        public Guid PersonId { get; init; }
        public string CompanyName { get; init; }
        public EmploymentType Type { get; init; }
        public decimal Salary { get; init; }
    }
    public Guid? AddEmployment(AddEmploymentCommand command)
    {
        var person = _context.People.Find(command.PersonId);
        if (person == null) return null;

        var employment = new Employment
        {
            Id = Guid.NewGuid(),
            CompanyName = command.CompanyName,
            Person = person,
            EmploymentType = command.Type,
            Salary = command.Salary
        };
        _context.Employments.Add(employment);
        return employment.Id;
    }
    // AddEmployment
    
    // READ
    public IEnumerable<Employment> GetEmployments()
    {
        var employments = _context.Employments.ToList();
        return employments.AsReadOnly();
    }

    public Employment? GetEmployment(Guid id)
    {
        var employment = _context.Employments.Find(id);
        return employment;
    }
    
    // UPDATE - props are immutable
    
    // DELETE
    public void RemoveEmployment(Guid employmentId)
    {
        var employment = _context.Employments.Find(employmentId);
        if (employment == null) return;
        _context.Employments.Remove(employment);
        _context.SaveChanges();
    }
    // RemoveEmployment
}