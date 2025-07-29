using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PocMissionPush.Infrastructure.Persistance;
using PocMissionPush.Loan;
using PocMissionPush.Subscriptions;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();
builder.Services.AddDbContext<SubscriptionDbContext>(opt => opt.UseInMemoryDatabase("SubscriptionList"));
builder.Services.AddDbContext<LoanDbContext>(opt => opt.UseInMemoryDatabase("LoanList"));


builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<PushService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<WorkSessionService>();
builder.Services.AddScoped<LoanService>();
builder.Services.AddHttpClient<KeycloakAdminService>();


JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://auth.ade-dev.fr/realms/ustest";
    options.RequireHttpsMetadata = true;
    // options.Audience = "us-mecanic";
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


// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();



app.UseCors("customPolicy");
app.UseHttpsRedirection();

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
    // using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
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


