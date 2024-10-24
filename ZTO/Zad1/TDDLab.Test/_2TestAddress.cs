using TDDLab.Core.InvoiceMgmt;
using Xunit;

//Chapter 2: Denial
namespace TDDLab.Test
{
    public class TestAddress
    {
        // I guess, these tests could be done using something like property-based testing; the logic should be tested once
        //  i.e. here we could multiply checks for different inputs ("", " ", null) - where the logic is: invalid input should return broken rule
        [Fact]
        public void AddressValidation_EmptyAddressLine_BreaksAddressLineRule()
        {
            // ...we can multiply test conditions per test
            Assert.Contains(Address.ValidationRules.AddressLine1, new Address("", "Legnickie Pole", "Dolnośląskie", "59-220").Validate());
            Assert.Contains(Address.ValidationRules.AddressLine1, new Address(null, "Legnickie Pole", "Dolnośląskie", "59-220").Validate());
        }

        [Fact]
        public void AddressValidation_EmptyCity_BreaksCityRule()
        {
            // but more expressive would be to define:
            // Assert.Contains(Address.ValidationRules.City, new Address("Słoneczna 12", InvalidCity, "Dolnośląskie", "59-220").Validate());
            Assert.Contains(Address.ValidationRules.City, new Address("Słoneczna 12", "", "Dolnośląskie", "59-220").Validate());
        }
        
        // an implementation of mentioned approach using [Theory]
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AddressValidation_EmptyState_BreaksStateRule(string invalidState)
        {
            Assert.Contains(Address.ValidationRules.State, new Address("Słoneczna 12", "Legnickie Pole", invalidState, "59-220").Validate());
        }
        
        // ...however all of previous tests are based on code I've read, not actual logic
        // what about more exhaustive tests? i.e. listing all edge cases we can think of
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")] // it's arguable if it's empty, or non-empty string
        [InlineData("\0")]
        [InlineData("\r")]
        [InlineData("\b")]
        [InlineData("\u3000")] //japanese space
        [InlineData("\u2800")] //Braille Pattern Blank
        public void AddressValidation_EmptyZip_BreaksZipRule(string invalidZip)
        {
            Assert.Contains(Address.ValidationRules.Zip, new Address("Słoneczna 12", "Legnickie Pole", "Dolnośląskie", invalidZip).Validate());
        }
        
        [Fact]
        public void AddressValidation_ValidAddress_BreaksNoRules()
        {
            // happy path
            Assert.Empty(new Address("Słoneczna 12", "Legnickie Pole", "Dolnośląskie", "59-220").Validate());
        }
    }
}