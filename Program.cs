using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlite(connStr));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/shorturl", async(UrlDto url, ApiDbContext db, HttpContext ctx) =>
{
    if (!Uri.TryCreate(url.Url, UriKind.Absolute, out var inputUrl))
        return Results.BadRequest("Invalid url has been provided");

    var random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@az";
    var randomStr = new string(Enumerable.Repeat(chars, 8).Select(c => c[random.Next(c.Length)]).ToArray());

    var sUrl = new UrlManagement()
    {
        Url = url.Url,
        ShorterUrl = randomStr
    };
    db.Urls.Add(sUrl);
    db.SaveChangesAsync();

    var result = $"{ctx.Request.Scheme}://{ctx.Request.Host}/{sUrl.ShorterUrl}";

    return Results.Ok(new UrlShortResponseDto()
    {
        Url = result
    });
});

app.Run();

class ApiDbContext : DbContext
{
    public virtual DbSet<UrlManagement> Urls { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {

    }
}