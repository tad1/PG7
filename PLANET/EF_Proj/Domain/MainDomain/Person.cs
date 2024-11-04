using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    
    public class Person
    {
        public Guid Id { get; set; }
        public Gender Gender { get; set; }
        
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
        public List<Employment> Employments { get; set; } = new List<Employment>();
        
        public Person? Father { get; set; }
        public Person? Mother { get; set; }
        public Person? Spouse { get; set; }
        
        public List<Person> Siblings { get; set; } = new List<Person>();
        
    }

    public class PublicPerson : Person
    {
        public decimal Fortune { get; set; }
    }
}