using Microsoft.EntityFrameworkCore;
using StogShared.Entities;

namespace StogLauncherApi;

public class ApplicationContext :DbContext
{
    public DbSet<DbPlayer> Users { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
