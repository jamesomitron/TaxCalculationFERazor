namespace TaxCalculationRazor.Entities;

public class TaxRecord
{
    public PostalCode PostalCode { get; set; }

    public decimal AnnualIncome { get; set; }

    public decimal CalculatedTaxValue { get; set; }
}
