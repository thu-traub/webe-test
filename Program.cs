// =========================================================
//  Demo App, Lecture Webengineering, S. Traub 2023
//
//  Simple database CRUD operations with SQL or JSON
// =========================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

// Set to true to include stuff for authentication via OAUTH

bool auth = true;

// Start by creating web Webapplication
var builder = WebApplication.CreateBuilder(args);

// We need to add environment variables to pass the client secret
builder.Configuration.AddEnvironmentVariables();

// Each app needs it's own app-id and client secret
// without auth, both variables become empty
string? appId = builder.Configuration.GetValue<String>("AzureAd:ClientId");
string? clientSecret =  builder.Configuration.GetValue<String>("AzureAd:ClientSecret");

// for authentication or database connection, we need a client secret
// You can delete this check, if you work with local files only
if (String.IsNullOrEmpty(clientSecret)) {
    System.Console.WriteLine("no client secret!");
    return;
}

// This is the connection to SQL Server
string? server =  builder.Configuration.GetValue<String>("SQLServer:host");
string? db =  builder.Configuration.GetValue<String>("SQLServer:database");
string cs = $"Server=tcp:{server},1433; Authentication=Active Directory Service Principal; Encrypt=True; Database={db}; User Id={appId}; Password={clientSecret}";

// For authentication, we need OpenIdConnect services
if (auth) {
    var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');
    builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
        .AddInMemoryTokenCaches();

    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = options.DefaultPolicy;
    });
}

if (auth) {
    builder.Services.AddRazorPages().AddMicrosoftIdentityUI();
} else {
    builder.Services.AddRazorPages();
}

// uncomment this line, if you need a backgriund worker
// builder.Services.AddHostedService<BackgroundWorker>();

// uncomment this line, if you want to work with json files only
// builder.Services.AddSingleton<IDataStore, JsonDataStore>();


// This is the service for database connections
builder.Services.AddDbContext<demo.Models.IfiStDbContext>(o=>o.UseSqlServer(cs).EnableSensitiveDataLogging());
builder.Services.AddScoped<IDataStore, SqlDataStore>();

var app = builder.Build();

// Now define the processing pipelinie

if (auth) {
    app.UseHttpsRedirection();
    app.UseRouting();
}

app.UseStaticFiles();

if (auth) app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();

if (auth) app.MapControllers();

app.Run();
