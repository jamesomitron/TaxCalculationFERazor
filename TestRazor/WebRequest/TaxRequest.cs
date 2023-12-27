using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text;
using TaxCalculationRazor.Entities;

namespace TaxCalculationRazor.WebRequest;

public class TaxRequest : ITaxRequest
{
    public async Task<IEnumerable<PostalCode>> GetPostalCodesAsync(HttpClient httpClient)
    {
        IEnumerable<PostalCode>? postalCodeList = Enumerable.Empty<PostalCode>();

        var httpResponseMessage = await httpClient.GetAsync("/api/postalcode");

        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            postalCodeList = await JsonSerializer.DeserializeAsync<IEnumerable<PostalCode>>(contentStream, options);
        }

        if (postalCodeList is null)
        {
            return Enumerable.Empty<PostalCode>();
        }

        return postalCodeList;
    }

    public async Task<IEnumerable<TaxRecord>> GetTaxRecordsAsync(HttpClient httpClient)
    {
        IEnumerable<TaxRecord>? TaxRecordList = Enumerable.Empty<TaxRecord>();

        var httpResponseMessage = await httpClient.GetAsync("/api/taxrecord");

        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            TaxRecordList = await JsonSerializer.DeserializeAsync<IEnumerable<TaxRecord>>(contentStream, options);
        }

        if (TaxRecordList is null)
        {
            return Enumerable.Empty<TaxRecord>();
        }

        return TaxRecordList;
    }

    public async Task<bool> PostTaxCalculationAsync(HttpClient httpClient, TaxRecordRequest taxRecordRequest)
    {
        TaxRecord? TaxRecord;

        var taxRecordRequestJson = new StringContent(JsonSerializer.Serialize(taxRecordRequest), Encoding.UTF8, Application.Json);

        using var httpResponseMessage = await httpClient.PostAsync("/api/calculatetax", taxRecordRequestJson);
        
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            TaxRecord = await JsonSerializer.DeserializeAsync<TaxRecord>(contentStream, options);

            return true;
        }

        return false;
    }
}
