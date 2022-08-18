using Dapper;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper.Contrib.Extensions;
using Websity.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GetConnection>(sp => async () => {
    var connection = new SqlConnection("Data Source=usinacompany.com;User ID=usina_usrmentoria;Password=Abc12345;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    await connection.OpenAsync();
    return connection;
});

var app = builder.Build();

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

app.MapGet("/alunos", async (GetConnection connectionGetter) =>
{
    var con = await connectionGetter();
    return con.GetAll<Aluno>().ToList();
});

app.MapGet("/alunos/{id}", async (GetConnection connectionGetter, Guid id) =>
{
    var con = await connectionGetter();
    return con.Get<Aluno>(id);
});

app.MapDelete("/alunos/{id}", async (GetConnection connectionGetter, Guid id) => {
    try {
        var con = await connectionGetter();
        con.Delete<Aluno>(new Aluno(id));
        return Results.Ok("Excluído com sucesso");
    } 
    catch (Exception e) {
        return Results.Problem("Erro durante a exclusão: " + e);
    }
});

app.MapPost("/alunos", async (GetConnection connectionGetter, Aluno aluno) => {
    var con = await connectionGetter();
    var id = con.Insert<Aluno>(aluno);
    return Results.Created($"/alunos/{id}", aluno);
});

app.MapPut("/alunos", async (GetConnection connectionGetter, Aluno aluno) => {
    var con = await connectionGetter();
    var id = con.Update<Aluno>(aluno);
});

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public delegate Task<IDbConnection> GetConnection();