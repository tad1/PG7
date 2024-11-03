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
    public struct AddPersonCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? FatherId { get; set; }
        public Guid? MotherId { get; set; }
    }
    public void AddPerson(AddPersonCommand command)
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = command.FirstName,
            Surname = command.LastName
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
        // UpdateName
        // UpdateSpouse

        // AddNumber
        // RemoveNumber
        // AddAddress
        // RemoveAddress
        
        // AddEmployment
        // RemoveEmployment
        
    // DELETE 
    public void DeletePerson(Guid id)
    {
        var person = _context.People.Find(id);
        if (person == null) return;
        _context.People.Remove(person);
    }

}

