using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class EmploymentType : ValueObject
{
        public string name;
        protected override IEnumerable<object> GetEqualityComponents()
        {
                yield return name;
        }
}

public class Employment
{
        public Guid Id { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public double Salary { get; set; }
        
        public Company? Company { get; set; }
        public Person? Person { get; set; }
}