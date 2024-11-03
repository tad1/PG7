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
    public record struct AddEmploymentCommand
    {
        public Guid PersonId { get; init; }
        public Guid CompanyId { get; init; }
        public EmploymentType Type { get; init; }
        public double Salary { get; init; }
    }
    public Guid? AddEmployment(AddEmploymentCommand command)
    {
        var person = _context.People.Find(command.PersonId);
        var company = _context.Companies.Find(command.CompanyId);
        if (company == null || person == null) return null;

        var employment = new Employment
        {
            Id = Guid.NewGuid(),
            Company = company,
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
    
    // UPDATE
    // UpdateSalary
    // UpdateEmploymentType
    
    // DELETE
    public void RemoveEmployment(Guid employmentId)
    {
        var employment = _context.Employments.Find(employmentId);
        if (employment == null) return;
        _context.Employments.Remove(employment);
    }
    // RemoveEmployment
}