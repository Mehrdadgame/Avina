namespace Avina.Models;

public record RegisterRequest(
    string Name,
    string Email,
    string Password
);

public record LoginRequest(
    string Email,
    string Password
);

public record RefreshTokenRequest(
    string RefreshToken
);

public record AuthResponse(
    bool Success,
    string Message,
    string? AccessToken = null,
    string? RefreshToken = null,
    UserAuthDto? User = null
);

public record UserAuthDto(
    int Id,
    string Name,
    string Email,
    string Role,
    int Coin,
    int Level,
    string? ProfileImage
);
