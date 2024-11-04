using System.Collections;
using Application.Interfaces;
using Domain;

namespace Application;

public class Queries
{
    IApplicationDbContext _context;

    public Queries(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public dynamic WithMostGrandChildren(Gender gender)
    {
        var results = _context.People.Select(person => new
        {
            PersonId = person.Id,
            NumberOfGrandChildren = _context.People
                .Where(child => child.Father == person || child.Mother == person)
                .SelectMany(child => _context.People
                    .Where(grandchild => grandchild.Father == person || grandchild.Mother == person))
                .Distinct()
                .Count(grandchild => grandchild.Gender == gender)
        }).OrderByDescending(v => v.NumberOfGrandChildren);
        return results.First();
    }
    
    public IEnumerable<dynamic> GetAvgCompanies()
    {
        return _context.Employments

            .GroupBy(e => e.EmploymentType.Name)
            .Select(g => new
            {
                EmploymentType = g.Key,
                TotalPeople = g.Count(),
                AvgSalary = g.Average(gr => gr.Salary)
            }).ToList();
    }

    /*znajdź osobę z najbogatszej rodziny (w skład rodziny wchodzi osoba,
    współmałżonek jeśli istnieje wszystkie dzieci i rodzice tej osoby i współmałżonka,
    należy uważać na powtórzenia, np. wspólne dziecko) */
    public dynamic GetPersonWithRichestFamily()
    {
        var peoplesFortune = _context.People.Select(person => new
            {
                PersonId = person.Id,
                TotalFortune = person.Employments.Sum(employment => employment.Salary) +
                               ((person is PublicPerson) ? ((person as PublicPerson)!).Fortune : 0),
            });

        var familyFortune = _context.People
            .Select(person => new
            {
                PersonId = person.Id,
                FamilyFortune = _context.People.Where(p =>
                    p.Id == person.Id ||
                    (person.Father != null && person.Father.Id == p.Id) ||
                    (person.Mother != null && person.Mother.Id == p.Id) ||
                    (person.Spouse != null && person.Spouse.Id == p.Id) ||
                    (person.Spouse != null && person.Spouse.Father != null && person.Spouse.Father.Id == p.Id) ||
                    (person.Spouse != null && person.Spouse.Mother != null && person.Spouse.Mother.Id == p.Id) ||
                    (p.Father != null && p.Father.Id == person.Id) ||
                    (p.Mother != null && p.Mother.Id == person.Id) ||
                    (p.Father != null && person.Spouse != null && p.Father.Id == person.Spouse.Id) ||
                    (p.Mother != null && person.Spouse != null && p.Mother.Id == person.Spouse.Id)
                ).Distinct().Join(peoplesFortune, leftPerson => leftPerson.Id, rightPerson => rightPerson.PersonId, (leftPerson, rightPerson) => new
                {
                    Fortune =  rightPerson.TotalFortune
                }).Sum(p => p.Fortune)
            });
        return familyFortune.OrderByDescending(v => v.FamilyFortune).ToList();
    }
}