namespace TaxCalculationRazor.Entities;

public class TaxRecordRequest
{
    public string? PostalCode { get; set; }
    public decimal AnnualIncome { get; set; }
}
