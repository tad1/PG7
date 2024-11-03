using System.ComponentModel.DataAnnotations;

namespace Domain;

public class PhoneNumber : ValueObject
{
    string Number { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}