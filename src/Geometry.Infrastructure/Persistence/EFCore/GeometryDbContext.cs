using Microsoft.EntityFrameworkCore;

namespace Geometry.Infrastructure.Persistence.EFCore;

/// <summary>
/// Entity Framework Core database context for the Geometry application.
/// Manages database connections and entity configurations.
/// </summary>
public class GeometryDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeometryDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public GeometryDbContext(DbContextOptions<GeometryDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DbSet for cube entities.
    /// </summary>
    public DbSet<CubeDBO> Cubes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Cylinder entities.
    /// </summary>
    public DbSet<CylinderDBO> Cylinders { get; set; } = null!;

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types
    /// exposed in <see cref="DbSet{TEntity}"/> properties on the derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Cube entity
        modelBuilder.Entity<CubeDBO>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SideLength)
                .IsRequired();
        });

        // Configure Cylinder entity
        modelBuilder.Entity<CylinderDBO>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Radius)
                .IsRequired();
            entity.Property(e => e.Height)
                .IsRequired();
        });
    }
}
