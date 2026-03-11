using Microsoft.AspNetCore.Mvc;

namespace BandHub.UserService.Features.Accounts.CreateAccount;

public static class RegisterAccountEndpoint
{
    public static IEndpointRouteBuilder MapCreateAccountEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/register", async (
            [FromBody] RegisterAccountRequest request,
            RegisterAccountHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/accounts/{response.Id}", response);
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
        .WithName("CreateAccount")
        .WithTags("Accounts");

        return app;
    }
}
