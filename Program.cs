using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UsNotificationApi.Application.Loans;
using UsNotificationApi.Application.Notifications;
using UsNotificationApi.Application.Subscriptions;
using UsNotificationApi.Application.Users;
using UsNotificationApi.Application.WorkSessions;
using UsNotificationApi.Domain.Loans;
using UsNotificationApi.Domain.Subscriptions;
using UsNotificationApi.Infrastructure.Keycloak;
using UsNotificationApi.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();
builder.Services.AddDbContext<SubscriptionDbContext>(opt => opt.UseInMemoryDatabase("SubscriptionList"));
builder.Services.AddDbContext<LoanDbContext>(opt => opt.UseInMemoryDatabase("LoanList"));

builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IPushService, PushService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IWorkSessionService, WorkSessionService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddHttpClient<IUserAdminService, KeycloakAdminService>();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://auth.ade-dev.fr/realms/ustest";
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidAudiences = new[] {
            "mecanic-api",
            "us-client"
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("customPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LoanDbContext>();
    LoadLoansFromCsv("Data/LoanInitData.csv", context);
}

app.Run();

static void LoadLoansFromCsv(string path, LoanDbContext context)
{
    if (!File.Exists(path)) return;

    using var reader = new StreamReader(path);
    using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("fr-FR"))
    {
        Delimiter = ";",
        TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
        IgnoreBlankLines = true,
        PrepareHeaderForMatch = args => args.Header.Trim()
    });

    var loans = csv.GetRecords<Loan>().ToList();
    context.Loans.AddRange(loans);
    context.SaveChanges();
}
