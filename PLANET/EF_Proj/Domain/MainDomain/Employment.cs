using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class EmploymentType : ValueObject
{
        public string Name { get; init; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
                yield return Name;
        }
}

public class Employment
{
        public Guid Id { get; init; }
        public EmploymentType EmploymentType { get; init; }
        public decimal Salary { get; init; }
        
        public string? CompanyName { get; init; }
        public Person Person { get; set; }
}