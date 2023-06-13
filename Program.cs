using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

var builder = WebApplication.CreateBuilder(args);

// Add endpoints API explorer
builder.Services.AddEndpointsApiExplorer();
// Add Swagger generation
builder.Services.AddSwaggerGen();

// Configure the connection string for the SQLite database
var connStr = "DataSource=app.db";
// Add the DbContext and configure it to use the SQLite connection string
builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlite(connStr));

var app = builder.Build();

// Enable Swagger and Swagger UI in the development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Handle the POST request to "/shorterurl"
app.MapPost("/shorterurl", async (UrlDto url, ApiDbContext db, HttpContext ctx) =>
{
    // Validate the provided URL
    if (!Uri.TryCreate(url.Url, UriKind.Absolute, out var inputUrl))
        return Results.BadRequest("Invalid URL has been provided");

    var random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@az";
    var randomStr = new string(Enumerable.Repeat(chars, 8).Select(c => c[random.Next(c.Length)]).ToArray());

    // Create a new UrlManagement object
    var sUrl = new UrlManagement()
    {
        Url = url.Url,
        ShorterUrl = randomStr
    };
    // Add the new UrlManagement object to the DbContext
    db.Urls.Add(sUrl);
    // Save the changes asynchronously
    await db.SaveChangesAsync();

    var result = $"{ctx.Request.Scheme}://{ctx.Request.Host}/{sUrl.ShorterUrl}";

    return Results.Ok(new UrlShortResponseDto()
    {
        Url = result
    });
});

// Handle fallback requests
app.MapFallback(async (ApiDbContext db, HttpContext ctx) =>
{
    var path = ctx.Request.Path.ToUriComponent().Trim('/');
    // Check if the requested URL exists in the database
    var urlMatch = await db.Urls.FirstOrDefaultAsync(c =>
        c.ShorterUrl.Trim() == path.Trim());

    if (urlMatch == null)
        return Results.BadRequest("Invalid request");

    // Redirect to the original URL
    return Results.Redirect(urlMatch.Url);
});

app.Run();

// DbContext for managing URLs
class ApiDbContext : DbContext
{
    public virtual DbSet<UrlManagement> Urls { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {

    }
}
