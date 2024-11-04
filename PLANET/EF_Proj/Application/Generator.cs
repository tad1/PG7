using System.Diagnostics;
using Application.Interfaces;
using Bogus;
using Domain;
using Person = Domain.Person;

namespace Application;

public class Generator
{
    
    IApplicationDbContext _context;

    private Faker<Person> personFaker;
    private Faker<PublicPerson> publicPersonFaker;
    private Faker<Employment> employmentFaker;
    private Faker<PhoneNumber> phoneNumberFaker;

    public Generator(IApplicationDbContext context)
    {
        _context = context;
        Randomizer.Seed = new Random(123); // for replication
        phoneNumberFaker = new Faker<PhoneNumber>()
            .RuleFor(p => p.Number, f => f.Phone.PhoneNumber("###-###-###"));
        personFaker = new Faker<Person>()
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Surname, f => f.Name.LastName())
            .RuleFor(p => p.PhoneNumbers, f => phoneNumberFaker.Generate(Random.Shared.Next(0,3)));

        publicPersonFaker = new Faker<PublicPerson>()
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Surname, f => f.Name.LastName())
            .RuleFor(p => p.Fortune, f => decimal.Round(f.Random.Decimal(1000, 60000)));
        
        employmentFaker = new Faker<Employment>()
            .RuleFor(e => e.EmploymentType, f => new EmploymentType()
            {
                Name = f.PickRandom((string[])["comission", "contract", "full-time", "part-time"])
            })
            .RuleFor(e => e.Salary, f => decimal.Round(f.Random.Decimal(1000, 10000), 2))
            .RuleFor(e => e.CompanyName, f => f.Company.CompanyName());
    }

    public void GeneratePeople(int numberOfPeople)
    {
        List<Person> people = new List<Person>();
        for (int i = 0; i < numberOfPeople; i++)
        {
            if (Random.Shared.NextDouble() > 0.8)
            {
                people.Add(publicPersonFaker.Generate());
            }
            else
            {
                people.Add(personFaker.Generate());
            }
        }
        (double single, double couples, double families) ratio = (20, 20, 60);
        (int single, int couples, int families) size;
        
        var totalRatio = ratio.single + ratio.couples + ratio.families;
        var total = numberOfPeople;
        size.families = (int)((ratio.families / totalRatio) * numberOfPeople);
        
        totalRatio = ratio.single + ratio.couples;
        total -= size.families;
        size.couples = (int)((ratio.couples / totalRatio) * total);
        size.couples -= size.couples % 2;
        
        total -= size.couples;
        size.single = total;
        
        _context.People.AddRange(people);
        _context.SaveChanges();
        
        var peopleLeft = new List<Person>(people);
        peopleLeft.RemoveRange(0, size.single);

        var couples = peopleLeft.Chunk(2).ToList();
        for (int i = 0; i < size.couples/2; i++)
        {
            var husband = couples[i][0];
            var wife = couples[i][1];
            
            husband.Gender = Gender.Male;
            husband.Spouse = wife;
            wife.Gender = Gender.Female;
            wife.Spouse = husband;
        }
        peopleLeft.RemoveRange(0, size.couples);
        
        List<int> familiesSizes = new List<int>();
        var left = peopleLeft.Count();
        while (left > 0)
        {
            var familySize = Random.Shared.Next(int.Min(left, 9), left);
            familiesSizes.Add(familySize);
            left -= familySize;
        }

        foreach (var familySize in familiesSizes)
        {
            var members = peopleLeft.Take(familySize).ToList();
            peopleLeft.RemoveRange(0, familySize);
                        
            var singleChildren = new HashSet<Person>();
            singleChildren.Add(members[0]);
            members.RemoveAt(0);

            while (members.Count > 0)
            {
                var roodId = Random.Shared.Next(singleChildren.Count);
                var root = singleChildren.ToList()[roodId];
                singleChildren.Remove(root);
                
                
                if(members.Count == 0) continue;
                root.Gender = Gender.Female;
                var spouce = members[0];
                members.RemoveAt(0);
                var rootSurname = Random.Shared.NextDouble() > 0.5 ? root.Surname : spouce.Surname;
                
                if(members.Count == 0) continue;
                var numberOfChildren = Random.Shared.Next(int.Min(members.Count, 2), int.Min(members.Count, 4));
                for (int i = 0; i < numberOfChildren; i++)
                {
                    var child = members[i];
                    child.Surname = rootSurname;
                    child.Mother = root;
                    child.Father = spouce;
                    singleChildren.Add(child);
                }
                members.RemoveRange(0, numberOfChildren);
            }
        }

        _context.SaveChanges();
    }

    public void GenerateEmployments(int numberOfEmployments)
    {
        var people = _context.People.ToArray();
        for (int i = 0; i < numberOfEmployments; i++)
        {
            var employment = employmentFaker.Generate();
            employment.Person = people[Random.Shared.Next(people.Length)];
            employment.Person.Employments.Add(employment);
            _context.Employments.Add(employment);
        }
        _context.SaveChanges();
    }
    
    public void Generate(int numberOfPeople, int numberOfEmployments)
    {
        GeneratePeople(numberOfPeople);
        GenerateEmployments(numberOfEmployments);
    }


    public void Clean()
    {
        _context.Employments.RemoveRange(_context.Employments);
        foreach (var person in _context.People.ToList())
        {
            _context.People.Remove(person);
            _context.SaveChanges();
        }
        _context.SaveChanges();
    }
}