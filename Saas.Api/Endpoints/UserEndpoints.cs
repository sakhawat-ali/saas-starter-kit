using Microsoft.AspNetCore.Mvc;
using Saas.Api.Models;
using Saas.Api.Services;

namespace Saas.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {

        var route = routes.MapGroup("/api/users")
            .WithTags("Users")
            .WithOpenApi();

        route.MapGet("/", GetUsers).WithName("GetUsers");
        route.MapGet("/{id:guid}", GetUser).WithName("GetUser");
        route.MapPost("/", CreateUser).WithName("CreateUser");
        route.MapPut("/{id:guid}", UpdateUser).WithName("UpdateUser");
        route.MapDelete("/{id:guid}", DeleteUser).WithName("DeleteUser");

        return route;
    }
    public static IResult GetUsers(DummyDataService dataService)
    {
        var users = dataService.GetUsers();
        return Results.Ok(users);
    }

    public static IResult GetUser(DummyDataService dataService, Guid id)
    {
        var user = dataService.GetUser(id);
        if (user == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(user);
    }

    public static IResult CreateUser(DummyDataService dataService, User user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Email))
        {
            return Results.BadRequest("User email is required.");
        }
        var createdUser = dataService.CreateUser(user);
        return Results.Created($"/api/users/{createdUser.Id}", createdUser);
    }

    public static IResult UpdateUser(DummyDataService dataService, Guid id, User user)
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
    }

    public static IResult DeleteUser(DummyDataService dataService, Guid id)
    {
        var deleted = dataService.DeleteUser(id);
        if (!deleted)
        {
            return Results.NotFound();
        }
        return Results.NoContent();
    }
}
