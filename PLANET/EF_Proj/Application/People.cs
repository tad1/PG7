using Application.Interfaces;
using Domain;

namespace Application;

public class People
{
    IApplicationDbContext _context;
    public People(IApplicationDbContext dbContext)
    {
        _context = dbContext;
    }
    
    // CREATE
    public class AddPersonCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; } = Gender.Other;
        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
        public Guid? FatherId { get; set; }
        public Guid? MotherId { get; set; }
    }
    public void AddPerson(AddPersonCommand command)
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Gender = command.Gender,
            Name = command.FirstName,
            Surname = command.LastName,
            PhoneNumbers = command.PhoneNumbers,
        };
        if (command.FatherId != null)
        {
            var father = _context.People.Find(command.FatherId);
            person.Father = father;
        }
        if (command.MotherId != null)
        {
            var mother = _context.People.Find(command.MotherId);
            person.Mother = mother;
        }
        
        _context.People.Add(person);
        _context.SaveChanges();
    }
    
    // READ
    public IEnumerable<Person> GetPeople()
    {
        var people = _context.People.ToList();
        return people.AsReadOnly();
    }

    public Person? GetPerson(Guid id)
    {
        var person = _context.People.Find(id);
        return person;
    }
    
    // UPDATE
    
    // Update Person:
    public class UpdatePersonCommand
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public Gender? Gender { get; set; } = null;
        public List<PhoneNumber>? PhoneNumbers { get; set; } = null;
        public Guid? FatherId { get; set; } = null;
        public Guid? MotherId { get; set; } = null;
    }
    public void Update(Guid id, UpdatePersonCommand command)
    {
        var person = _context.People.Find(id);
        if(person == null) return;

        if (command.FirstName != null)
        {
            person.Name = command.FirstName;
        }

        if (command.LastName != null)
        {
            person.Surname = command.LastName;
        }

        if (command.Gender != null)
        {
            person.Gender = command.Gender.Value;
        }

        if (command.PhoneNumbers != null)
        {
            person.PhoneNumbers = command.PhoneNumbers;
        }

        if (command.FatherId != null)
        {
            var father = _context.People.Find(command.FatherId);
            if (father == null) return;
            person.Father = father;
        }

        if (command.MotherId != null)
        {
            var mother = _context.People.Find(command.MotherId);
            if (mother == null) return;
            person.Mother = mother;
        }
        _context.SaveChanges();
    }
        
    // DELETE 
    public void DeletePerson(Guid id)
    {
        var person = _context.People.Find(id);
        if (person == null) return;
        _context.Employments.RemoveRange(person.Employments);
        _context.SaveChanges();
        _context.People.Remove(person);
        _context.SaveChanges();
    }

}

