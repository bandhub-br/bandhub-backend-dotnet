namespace BandHub.UserService.Features.Users.GetUsers;

public static class GetUsersEndpoint
{
    public static IEndpointRouteBuilder MapGetUsersEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (
            GetUsersHandler handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler.HandleAsync(cancellationToken);
            return Results.Ok(response);
        })
        .WithName("GetUsers")
        .WithTags("Users");

        return app;
    }
}