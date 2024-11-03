using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Company
{
    public Guid id { get; set; }
    public string name { get; set; }
    
    public List<Employment> employments { get; set; }    
}