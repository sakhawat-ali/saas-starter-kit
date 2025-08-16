namespace Saas.Api.Models;

public class TenantContext
{
    public string TenantId { get; set; } = "demo";
    public string TenantName { get; set; } = "Demo Tenant";
    public string Plan { get; set; } = "Free";
}

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public class DashboardStats
{
    public string TenantId { get; set; } = string.Empty;
    public int TotalProjects { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveProjects { get; set; }
    public decimal CurrentMonthUsage { get; set; }
    public string PlanName { get; set; } = string.Empty;
}
