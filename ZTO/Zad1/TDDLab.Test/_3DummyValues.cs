using System.Collections.Generic;
using TDDLab.Core.InvoiceMgmt;

namespace TDDLab.Test
{
    public class _3ExampleValues
    {
        private static Address invalidAddress = new Address("", "", "", "");
        private static Address validAddress = new Address("Plane 12", "Planes", "Compat", "123-455");
        private static Recipient invalidRecipient = new Recipient("", invalidAddress);
        private static Recipient validRecipient = new Recipient("Jim Jim", validAddress);
        private static Money invalidMoney = new Money(0, null);
        private static Money validMoney = new Money(10, "PLN");
        private static InvoiceLine invalidInvoiceLine = new InvoiceLine("Proudct", invalidMoney);
        private static InvoiceLine validInvoiceLine = new InvoiceLine("Proudct", validMoney);
        private static IEnumerable<InvoiceLine> invalidLinesArray = new InvoiceLine[] {invalidInvoiceLine};
        private static IEnumerable<InvoiceLine> validLinesArray = new InvoiceLine[] {validInvoiceLine};
        private static Invoice invalidInvoice = new Invoice("", invalidRecipient, invalidAddress, invalidLinesArray);
        private static Invoice validInvoice = new Invoice("K12/1234", validRecipient, validAddress, validLinesArray);
    }
}