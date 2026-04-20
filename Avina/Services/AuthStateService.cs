using System.Text.Json;
using Microsoft.JSInterop;

namespace Avina.Services;

public class UserCacheDto
{
    public int Id { get; set; } = 0;
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
    private List<Func<Task>> _listeners = new();
    private const string StorageKey = "avina_user";

    public UserCacheDto? CurrentUser
    {
        get => _currentUser;
        private set => _currentUser = value;
    }

    public bool IsLoggedIn => _currentUser != null;

    public event Func<Task>? StateChanged;

    public void SetUser(UserCacheDto user)
    {
        Console.WriteLine($"[AuthState] Setting user: {user.Name}");
        CurrentUser = user;
        NotifyStateChanged();
    }

    public void Logout()
    {
        Console.WriteLine("[AuthState] Logging out");
        CurrentUser = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        Console.WriteLine($"[AuthState] NotifyStateChanged - IsLoggedIn: {IsLoggedIn}");
        StateChanged?.Invoke();
    }
}
