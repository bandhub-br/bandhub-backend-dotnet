namespace BandHub.BandService.Features.Bands.CreateBand;

public sealed record CreateBandRequest(string Name, string Description, string Genre, string? SpotifyId);