using Microsoft.AspNetCore.Mvc;

namespace BandHub.UserService.Features.Users.CreateUser;

public static class CreateUserEndpoint
{
    public static IEndpointRouteBuilder MapCreateUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (
            [FromBody] CreateUserRequest request,
            CreateUserHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/users/{response.Id}", response);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
        })
        .WithName("CreateUser")
        .WithTags("Users");

        return app;
    }
}