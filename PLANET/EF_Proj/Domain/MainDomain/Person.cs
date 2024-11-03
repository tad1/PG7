using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Person
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
        public List<Employment> Employments { get; set; } = new List<Employment>();
        public List<Address> Addresses { get; set; } = new List<Address>();
        
        public Person? Father { get; set; }
        public Person? Mother { get; set; }
        public Person? Spouse { get; set; }
        
        public List<Person> Siblings { get; set; } = new List<Person>();
    }
}