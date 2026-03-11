namespace BandHub.UserService.Features.Accounts.GetAccounts;

public static class GetAccountsEndpoint
{
    public static IEndpointRouteBuilder MapGetAccountsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/accounts/getaccountbyemail", async (
            string email,
            GetAccountsHandler handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler.HandleAsync(email, cancellationToken);
            return Results.Ok(response);
        })
        .WithName("GetAccounts")
        .WithTags("Accounts");

        return app;
    }
}
