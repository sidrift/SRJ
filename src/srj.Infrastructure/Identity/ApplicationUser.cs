using Microsoft.AspNetCore.Identity;

namespace srj.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;


    public bool IsApproved { get; set; } = false;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public DateTime? ApprovedAt { get; set; }


    public string? ApprovedBy { get; set; }
}