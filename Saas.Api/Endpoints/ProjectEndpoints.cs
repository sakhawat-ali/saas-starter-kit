using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Saas.Api.Models;
using Saas.Api.Services;

namespace Saas.Api.Endpoints;

public static class ProjectEndpoints
{
    public static RouteGroupBuilder MapProjectEndpoints(this IEndpointRouteBuilder routes)
    {
        var route = routes.MapGroup("/api/projects")
                          .WithTags("Projects")
                          .WithOpenApi();

        route.MapGet("/", GetProjects).WithName("GetProjects");
        route.MapGet("/{id:guid}", GetProject).WithName("GetProject");
        route.MapPost("/", CreateProject).WithName("CreateProject");
        route.MapPut("/{id:guid}", UpdateProject).WithName("UpdateProject");
        route.MapDelete("/{id:guid}", DeleteProject).WithName("DeleteProject");

        return route;
    }
    public static IResult GetProjects(DummyDataService dataService)
    {
        var projects = dataService.GetProjects();
        return Results.Ok(projects);
    }

    public static IResult GetProject(DummyDataService dataService, Guid id)
    {
        var project = dataService.GetProject(id);
        if (project == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(project);
    }

    public static IResult CreateProject(DummyDataService dataService, Project project)
    {
        if (project == null || string.IsNullOrWhiteSpace(project.Name))
        {
            return Results.BadRequest("Project name is required.");
        }
        var createdProject = dataService.CreateProject(project);
        return Results.Created($"/api/projects/{createdProject.Id}", createdProject);
    }

    public static IResult UpdateProject(DummyDataService dataService, Guid id, Project project)
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
    }

    public static IResult DeleteProject(DummyDataService dataService, Guid id)
    {
        var deleted = dataService.DeleteProject(id);
        if (!deleted)
        {
            return Results.NotFound();
        }
        return Results.NoContent();
    }
}
