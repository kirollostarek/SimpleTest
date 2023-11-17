using Microsoft.EntityFrameworkCore;

namespace SimpleUserAPIs.Models;

public partial class SimpleUserDbContext : DbContext
{
    public SimpleUserDbContext()
    {
    }

    public SimpleUserDbContext(DbContextOptions<SimpleUserDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SimpleUser> SimpleUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KIRO;Database=SimpleUserDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimpleUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SimpleUser");

            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.MarketingConsent).HasColumnName("marketingConsent");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
