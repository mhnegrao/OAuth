var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
ConfigureServices(builder);

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "OAuth";
    })
    .AddOAuth("OAuth", options =>
    {
        options.ClientId = "<SEU_CLIENT_ID>";
        options.ClientSecret = "<SEU_CLIENT_SECRET>";
        options.AuthorizationEndpoint = "https://oauth.bancodobrasil.com.br/oauth/authorize";
        options.TokenEndpoint = "https://oauth.bancodobrasil.com.br/oauth/token";
        options.CallbackPath = new PathString("/signin-bb");

        // Configuração do escopo conforme necessidade da API de cobranças
        options.Scope.Add("coberturas");
        options.Scope.Add("pagamento");

        options.SaveTokens = true;
    }
    );
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

