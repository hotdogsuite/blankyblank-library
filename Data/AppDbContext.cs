using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlankyBlankLibrary.Data;

public class AppDbContext : IdentityDbContext<IdentityUser> {

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Models.Password> Passwords { get; set; } = null!;
    public DbSet<Models.SentenceStructure> Structures { get; set; } = null!;
    public DbSet<Models.Word> Words { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Entity<Data.Models.Word.WordWord>().HasOne(nameof(Data.Models.Word.WordWord.LinkedWord)).WithMany();
        base.OnModelCreating(builder);
    }

}