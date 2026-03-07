namespace BandHub.BandService.Features.Bands.GetBands;

public sealed record GetBandsResponse(Guid Id, string Name, string Genre, string Description, string? SpotifyId, DateTime CreatedAt);
