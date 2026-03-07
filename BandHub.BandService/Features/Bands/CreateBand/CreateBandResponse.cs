namespace BandHub.BandService.Features.Bands.CreateBand;

public sealed record CreateBandResponse(Guid Id, string Name, string Genre, string Description, string? SpotifyId, DateTime CreateAt);
