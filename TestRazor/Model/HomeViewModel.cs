using System.ComponentModel.DataAnnotations;

namespace TestRazor.Model;

public class HomeViewModel
{
    [Display(Name = "Annual Income:")]
    [Required(ErrorMessage = "Annual Income is required.")]
    [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Valid Annual Income should have 2 decimal places.")]
    public string AnnualIncome { get; set; }
    public string Code { get; set; }
}
