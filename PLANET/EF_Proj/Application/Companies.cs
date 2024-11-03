using Application.Interfaces;
using Domain;

namespace Application;

public class Companies
{
    IApplicationDbContext _context;

    public Companies(IApplicationDbContext context)
    {
        _context = context;
    }
    
    // CREATE
    public void AddCompany(string name)
    {
        var company = new Company
        {
            id = Guid.NewGuid(),
            employments = new List<Employment>(),
            name = name
        };
        _context.Companies.Add(company);
    }
    
    // READ
    public IEnumerable<Company> GetCompanies()
    {
        var companies = _context.Companies.ToList();
        return companies.AsReadOnly();
    }

    public Company? GetCompany(Guid id)
    {
        var company = _context.Companies.FirstOrDefault(x => x.id == id);
        return company;
    }
    
    // UPDATE
    public void UpdateName(Guid id, string name)
    {
        var company = _context.Companies.Find(id);
        if (company == null) return;
        company.name = name;
    }
    
    // DELETE
    public void DeleteCompany(Guid id)
    {
        var company = _context.Companies.Find(id);
        if (company == null) return;
        _context.Companies.Remove(company);
    }    
}