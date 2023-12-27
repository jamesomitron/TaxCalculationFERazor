using Ardalis.GuardClauses;
using TaxCalculationRazor.WebRequest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var BaseURL = builder.Configuration.GetValue<string>("AppSettings:BaseUrl");
Guard.Against.Null(BaseURL, message: "Base URL 'BaseUrl' not found.");

builder.Services.AddHttpClient("TaxCalculationAPI", httpClient =>
{
    httpClient.BaseAddress = new Uri(BaseURL);
});

builder.Services.AddScoped<ITaxRequest, TaxRequest>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
