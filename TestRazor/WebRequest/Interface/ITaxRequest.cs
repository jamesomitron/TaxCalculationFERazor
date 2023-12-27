using TaxCalculationRazor.Entities;

namespace TaxCalculationRazor.WebRequest;

public interface ITaxRequest
{
    Task<IEnumerable<PostalCode>> GetPostalCodesAsync(HttpClient httpClient);
    Task<IEnumerable<TaxRecord>> GetTaxRecordsAsync(HttpClient httpClient);
    Task<bool> PostTaxCalculationAsync(HttpClient httpClient, TaxRecordRequest taxRecordRequest);
}
