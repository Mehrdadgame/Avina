namespace Avina.Services;

public class UserCacheDto
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "دانش‌آموز";
    public string Bio { get; set; } = "";
    public string ProfileImage { get; set; } = "https://i.pravatar.cc/150?img=5";
    public int Coin { get; set; } = 0;
}

public class AuthStateService
{
    private UserCacheDto? _currentUser;

    public UserCacheDto? CurrentUser => _currentUser;
    public bool IsLoggedIn => _currentUser != null;

    public event Action? StateChanged;

    public void SetUser(UserCacheDto user)
    {
        _currentUser = user;
        StateChanged?.Invoke();
    }

    public void Logout()
    {
        _currentUser = null;
        StateChanged?.Invoke();
    }
}
