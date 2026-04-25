namespace Avina.Services;

public class UserCacheDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "دانش‌آموز";
    public string Bio { get; set; } = string.Empty;
    public string ProfileImage { get; set; } = "/images/avatars/student-default.svg";
    public int Coin { get; set; }
}

public class AuthStateService
{
    private UserCacheDto? _currentUser;

    public UserCacheDto? CurrentUser
    {
        get => _currentUser;
        private set => _currentUser = value;
    }

    public bool IsLoggedIn => _currentUser is not null;

    public event Func<Task>? StateChanged;

    public void SetUser(UserCacheDto user)
    {
        CurrentUser = user;
        NotifyStateChanged();
    }

    public void Logout()
    {
        CurrentUser = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}
