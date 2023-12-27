using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Http;
using TaxCalculationRazor.Entities;
using TaxCalculationRazor.WebRequest;
using TestRazor.Model;

namespace TestRazor.Pages
{
    public class IndexModel : PageModel
    {
        
        [BindProperty]
        public HomeViewModel HomeViewModel { get; set; }
        public SelectList options;
        public IEnumerable<TaxRecord> _taxRecords;

        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly ITaxRequest _taxRequest;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory, ITaxRequest taxRequest)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _taxRequest = taxRequest;
            _taxRecords = Enumerable.Empty<TaxRecord>();
            _httpClient = _httpClientFactory.CreateClient("TaxCalculationAPI");
        }

        public async Task OnGetAsync()
        {
            try
            {
                await LoadSelects();
                await LoadTaxRecords();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task OnPostAsync()
        {
            if(!ValidateAnnualIncome(HomeViewModel.AnnualIncome))
            {
                ModelState.AddModelError("HomeViewModel.AnnualIncome", $"Invalid Annual Income Entered - {HomeViewModel.AnnualIncome}");
            }

            if(HomeViewModel.Code.Equals("Select Postal Code"))
            {
                ModelState.AddModelError("HomeViewModel.Code", "Select A Postal Code");
            }

            if (ModelState.IsValid)
            {
                await _taxRequest.PostTaxCalculationAsync(_httpClient, new TaxRecordRequest { PostalCode = HomeViewModel.Code, AnnualIncome = Convert.ToDecimal(HomeViewModel.AnnualIncome) });
                HomeViewModel.AnnualIncome = string.Empty;
            }

            await LoadSelects();
            await LoadTaxRecords();
        }

        private async Task LoadSelects()
        {
            var postalCode = await _taxRequest.GetPostalCodesAsync(_httpClient);

            options = new SelectList(postalCode.Select(x => x.Code).ToList());
        }

        private async Task LoadTaxRecords()
        {
            var taxRecords = await _taxRequest.GetTaxRecordsAsync(_httpClient);
            _taxRecords = taxRecords;
        }

        private bool ValidateAnnualIncome(string annualIncome)
        {
            Decimal number;
            int intNumber;

            if (Decimal.TryParse(annualIncome, out number))
            {
                var ceilingAnnual = Math.Ceiling(number).ToString();

                if (Int32.TryParse(ceilingAnnual, out intNumber))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
