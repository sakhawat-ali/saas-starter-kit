using Microsoft.AspNetCore.Mvc;
using Saas.Api.Models;
using Saas.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DummyDataService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var projectMapGroup = app.MapGroup("/api/projects")
    .WithTags("Projects")
    .WithOpenApi();

var userMapGroup = app.MapGroup("/api/users")
    .WithTags("Users")
    .WithOpenApi();

#region /api/projects

projectMapGroup.MapGet("/", ([FromServices] DummyDataService dataService) =>
{
    var projects = dataService.GetProjects();
    return Results.Ok(projects);
}).WithName("GetProjects");

projectMapGroup.MapGet("/{id:guid}", ([FromServices] DummyDataService dataService, Guid id) =>
{
    var project = dataService.GetProject(id);
    if (project == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(project);
}).WithName("GetProject");

projectMapGroup.MapPost("/", ([FromServices] DummyDataService dataService, [FromBody] Project project) =>
{
    if (project == null || string.IsNullOrWhiteSpace(project.Name))
    {
        return Results.BadRequest("Project name is required.");
    }
    var createdProject = dataService.CreateProject(project);
    return Results.Created($"/api/projects/{createdProject.Id}", createdProject);
}).WithName("CreateProject");

projectMapGroup.MapPut("/{id:guid}", ([FromServices] DummyDataService dataService, Guid id, [FromBody] Project project) =>
{
    if (project == null || string.IsNullOrWhiteSpace(project.Name))
    {
        return Results.BadRequest("Project name is required.");
    }
    var updatedProject = dataService.UpdateProject(id, project);
    if (updatedProject == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(updatedProject);
}).WithName("UpdateProject");

projectMapGroup.MapDelete("/{id:guid}", ([FromServices] DummyDataService dataService, Guid id) =>
{
    var deleted = dataService.DeleteProject(id);
    if (!deleted)
    {
        return Results.NotFound();
    }
    return Results.NoContent();
}).WithName("DeleteProject");

#endregion

#region /api/users

userMapGroup.MapGet("/", ([FromServices] DummyDataService dataService) =>
{
    var users = dataService.GetUsers();
    return Results.Ok(users);
}).WithName("GetUsers");

userMapGroup.MapGet("/{id:guid}", ([FromServices] DummyDataService dataService, Guid id) =>
{
    var user = dataService.GetUser(id);
    if (user == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(user);
}).WithName("GetUser");

userMapGroup.MapPost("/", ([FromServices] DummyDataService dataService, [FromBody] User user) =>
{
    if (user == null || string.IsNullOrWhiteSpace(user.Email))
    {
        return Results.BadRequest("User email is required.");
    }
    var createdUser = dataService.CreateUser(user);
    return Results.Created($"/api/users/{createdUser.Id}", createdUser);
}).WithName("CreateUser");


userMapGroup.MapPut("/{id:guid}", ([FromServices] DummyDataService dataService, Guid id, [FromBody] User user) =>
{
    if (user == null || string.IsNullOrWhiteSpace(user.Email))
    {
        return Results.BadRequest("User email is required.");
    }
    var updatedUser = dataService.UpdateUser(id, user);
    if (updatedUser == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(updatedUser);
}).WithName("UpdateUser");

userMapGroup.MapDelete("/{id:guid}", ([FromServices] DummyDataService dataService, Guid id) =>
{
    var deleted = dataService.DeleteUser(id);
    if (!deleted)
    {
        return Results.NotFound();
    }
    return Results.NoContent();
}).WithName("DeleteUser");

#endregion

app.Run();

