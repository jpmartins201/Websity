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
    var connection = new SqlConnection("");
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
        var deleted = await con.ExecuteAsync("DELETE FROM Aluno WHERE Id = @id", new {Id = id.ToString()});
        return Results.Ok();
    } 
    catch (Exception e) {
        return Results.Problem("Erro durante a exclusão: " + e);
    }
});

app.MapPost("/alunos", async (GetConnection connectionGetter, Aluno aluno) => {
    var con = await connectionGetter();
    var id = con.Execute("INSERT INTO Aluno (Id, Nome, Email, Cpf) VALUES (@Id, @Nome, @Email, @Cpf)", aluno);
    return Results.Created($"/alunos/{id}", aluno);
});

app.MapPut("/alunos", async (GetConnection connectionGetter, Aluno aluno) => {
    var con = await connectionGetter();
    var id = con.Execute("UPDATE Aluno SET Nome=@Nome, Email=@Email, Cpf=@Cpf WHERE AlunoId = @Id", aluno);
    return Results.Created($"/alunos/{id}", aluno);
    // var id = con.Update<Aluno>(aluno);
});

app.MapGet("/categorias", async (GetConnection connectionGetter) =>
{
    var con = await connectionGetter();
    var result = con.GetAll<Categoria>();
    return Results.Ok(result);
});

app.MapGet("/categorias/{id}", async (GetConnection connectionGetter, Guid id) =>
{
    var con = await connectionGetter();
    return con.Get<Categoria>(id);
});

app.MapPost("/categorias", async (GetConnection connectionGetter, Categoria categoria) => {
    var con = await connectionGetter();
    // var id = con.Insert<Categoria>(categoria);
    var id = con.Execute("INSERT INTO Categoria VALUES (@Id, @Titulo, @Descricao)", categoria);
    return Results.Created($"/categorias/{id}", categoria);
});

app.MapPut("/categorias", async (GetConnection connectionGetter, Categoria categoria) => {
    var con = await connectionGetter();
    var id = con.Execute("UPDATE Categoria SET Titulo = @Titulo, Descricao = @Descricao WHERE Id = @Id", categoria);
    return Results.Ok();
    // var id = con.Update<Categoria>(categoria);
});

app.MapDelete("/categorias/{id}", async (GetConnection connectionGetter, Guid id) => {
    try {
        var con = await connectionGetter();
        var _ = con.ExecuteAsync("DELETE FROM Categoria WHERE Id = @Id", new {Id = id.ToString()});
        return Results.Ok("Excluído com sucesso");
    }
    catch (Exception e) {
        return Results.Problem("Erro durante a exclusão: " + e);
    }
});

app.MapGet("/autors", async (GetConnection connectionGetter) =>
{
    var con = await connectionGetter();
    return con.GetAll<Autor>().ToList();
});

app.MapGet("/autors/{id}", async (GetConnection connectionGetter, Guid id) =>
{
    var con = await connectionGetter();
    return con.Get<Autor>(id);
});

app.MapDelete("/autor/{id}", async (GetConnection connectionGetter, Guid id) => {
    try {
        var con = await connectionGetter();
        con.Delete<Autor>(new Autor(id));
        return Results.Ok("Excluído com sucesso");
    }
    catch (Exception e) {
        return Results.Problem("Erro durante a exclusão: " + e);
    }
});

app.MapPost("/autors", async (GetConnection connectionGetter, Autor autor) => {
    var con = await connectionGetter();
    var id = con.Insert<Autor>(autor);
    return Results.Created($"/autor/{id}", autor);
});

app.MapPut("/autor", async (GetConnection connectionGetter, Autor autor) => {
    var con = await connectionGetter();
    var id = con.Update<Autor>(autor);
});


app.MapGet("/cursos", async (GetConnection connectionGetter) =>
{
    var con = await connectionGetter();
    return con.GetAll<Curso>().ToList();
});

app.MapGet("/cursos/{id}", async (GetConnection connectionGetter, Guid id) =>
{
    var con = await connectionGetter();
    return con.Get<Curso>(id);
});

app.MapDelete("/cursos/{id}", async (GetConnection connectionGetter, Guid id) => {
    try {
        var con = await connectionGetter();
        con.Execute("DELETE FROM AlunoCurso WHERE CoursoId = @id");
        con.Delete<Curso>(new Curso(id));
        return Results.Ok("Excluído com sucesso");
    }
    catch (Exception e) {
        return Results.Problem("Erro durante a exclusão: " + e);
    }
});

app.MapPost("/cursos", async (GetConnection connectionGetter, Curso curso) => {
    var con = await connectionGetter();
    var id = con.Insert<Curso>(curso);
    return Results.Created($"/curso/{id}", curso);
});

app.MapPut("/cursos", async (GetConnection connectionGetter, Curso curso) => {
    var con = await connectionGetter();
    var id = con.Update<Curso>(curso);
});

// ALUNO-CURSO
app.MapGet("/classes", async (GetConnection connectionGetter) =>
{
    var con = await connectionGetter();
    return con.GetAll<AlunoCurso>().ToList();
});

app.MapGet("/classes/{cursoId}/", async (GetConnection connectionGetter, Guid id) =>
{
    var con = await connectionGetter();
    return con.Get<AlunoCurso>(id);
});

app.MapDelete("/classes/{id}", async (GetConnection connectionGetter, Guid id) => {
    try {
        var con = await connectionGetter();
        con.Execute("DELETE FROM AlunoCurso WHERE CoursoId = @id");
        return Results.Ok("Excluído com sucesso");
    }
    catch (Exception e) {
        return Results.Problem("Erro durante a exclusão: " + e);
    }
});

app.MapPost("/classes", async (GetConnection connectionGetter, AlunoCurso matricula) => {
    var con = await connectionGetter();
    Console.Write(matricula.CoursoId);
    var class_id = con.Insert<AlunoCurso>(matricula);
    return Results.Created($"/classes/{class_id}", matricula);
});

app.MapPut("/classes", async (GetConnection connectionGetter, AlunoCurso matricula) => {
    var con = await connectionGetter();
    var id = con.Update<AlunoCurso>(matricula);
});


app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public delegate Task<IDbConnection> GetConnection();
