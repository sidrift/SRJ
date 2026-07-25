namespace srj.Blazor.Services;

public class TokenService
{
    private DateTime _expiry;
    private string? _token;
    private string? _userName;

    public void SetToken(string token, string userName, DateTime expiry)
    {
        _token = token;
        _userName = userName;
        _expiry = expiry;
    }

    public string? GetToken()
    {
        return _token;
    }

    public string? GetUserName()
    {
        return _userName;
    }

    public DateTime GetExpiry()
    {
        return _expiry;
    }

    public void Clear()
    {
        _token = null;
        _userName = null;
    }
}