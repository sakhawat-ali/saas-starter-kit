using Microsoft.AspNetCore.Http.HttpResults;
using Saas.Api.Models;

namespace Saas.Api.Services;


public class DummyDataService
{

    private readonly List<Project> _projects =
    [
        new Project
        {
            Id = Guid.NewGuid(),
            Name = "Demo Project Alpha",
            Description = "First demo project for testing API endpoints",
            TenantId = "demo",
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            CreatedBy = "admin@demo.com",
            IsActive = true
        },
        new Project
        {
            Id = Guid.NewGuid(),
            Name = "Demo Project Beta",
            Description = "Second demo project with more features",
            TenantId = "demo",
            CreatedAt = DateTime.UtcNow.AddDays(-15),
            CreatedBy = "user@demo.com",
            IsActive = true
        },
        new Project
        {
            Id = Guid.NewGuid(),
            Name = "Demo Project Gamma",
            Description = "Third demo project (archived)",
            TenantId = "demo",
            CreatedAt = DateTime.UtcNow.AddDays(-60),
            CreatedBy = "admin@demo.com",
            IsActive = false
        }
    ];

    private readonly List<User> _users =
    [
        new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@demo.com",
            FirstName = "Admin",
            LastName = "User",
            Role = "Owner",
            TenantId = "demo",
            CreatedAt = DateTime.UtcNow.AddDays(-90),
            IsActive = true
        },
        new User
        {
            Id = Guid.NewGuid(),
            Email = "user@demo.com",
            FirstName = "Regular",
            LastName = "User",
            Role = "Member",
            TenantId = "demo",
            CreatedAt = DateTime.UtcNow.AddDays(-45),
            IsActive = true
        },
        new User
        {
            Id = Guid.NewGuid(),
            Email = "manager@demo.com",
            FirstName = "Project",
            LastName = "Manager",
            Role = "Admin",
            TenantId = "demo",
            CreatedAt = DateTime.UtcNow.AddDays(-20),
            IsActive = true
        }
    ];

    private readonly TenantContext _tenantContext = new();
    public TenantContext GetTenantContext() => _tenantContext;

    #region Projects
    public IEnumerable<Project> GetProjects() => _projects.Where(p => p.TenantId == _tenantContext.TenantId);

    public Project? GetProject(Guid id) => _projects.FirstOrDefault(p => p.Id == id && p.TenantId == _tenantContext.TenantId);

    public Project CreateProject(Project project)
    {
        if (project == null) throw new ArgumentNullException(nameof(project));
        project.Id = Guid.NewGuid();
        project.TenantId = _tenantContext.TenantId;
        project.CreatedAt = DateTime.UtcNow;
        project.CreatedBy = "admin@demo.com";
        _projects.Add(project);
        return project;
    }

    public Project UpdateProject(Guid id, Project project)
    {
        if (project == null) throw new ArgumentNullException(nameof(project));
        var existing = GetProject(id);
        if (existing == null) throw new KeyNotFoundException($"Project with ID {project.Id} not found.");

        existing.Name = project.Name;
        existing.Description = project.Description;
        existing.IsActive = project.IsActive;
        return existing;
    }

    public bool DeleteProject(Guid id)
    {
        var project = GetProject(id);
        if (project == null) throw new KeyNotFoundException($"Project with ID {id} not found.");

        return _projects.Remove(project);
    }

    #endregion
    #region Users

    public IEnumerable<User> GetUsers() => _users.Where(u => u.TenantId == _tenantContext.TenantId);

    public User? GetUser(Guid id) => _users.FirstOrDefault(u => u.Id == id && u.TenantId == _tenantContext.TenantId);

    public User CreateUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        user.Id = Guid.NewGuid();
        user.TenantId = _tenantContext.TenantId;
        user.CreatedAt = DateTime.UtcNow;
        _users.Add(user);
        return user;
    }

    public User UpdateUser(Guid id, User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        var existing = GetUser(id);
        if (existing == null) throw new KeyNotFoundException($"User with ID {user.Id} not found.");

        existing.Email = user.Email;
        existing.FirstName = user.FirstName;
        existing.LastName = user.LastName;
        existing.Role = user.Role;
        existing.IsActive = user.IsActive;
        return existing;
    }

    public bool DeleteUser(Guid id)
    {
        var user = GetUser(id);
        if (user == null) throw new KeyNotFoundException($"User with ID {id} not found.");

        return _users.Remove(user);
    }
    #endregion
    public DashboardStats GetDashboardStats()
    {
        var tenantProjects = GetProjects().ToList();
        var tenantUsers = GetUsers().ToList();

        return new DashboardStats
        {
            TenantId = _tenantContext.TenantId,
            TotalProjects = tenantProjects.Count,
            TotalUsers = tenantUsers.Count,
            ActiveProjects = tenantProjects.Count(p => p.IsActive),
            CurrentMonthUsage = 1250.75m, // Hardcoded usage amount
            PlanName = _tenantContext.Plan
        };
    }
}