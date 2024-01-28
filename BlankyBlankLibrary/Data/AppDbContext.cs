using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlankyBlankLibrary.Data;

public class AppDbContext : DbContext {

    public DbSet<Models.Passwords.Password> Passwords { get; set; }
    public DbSet<Models.Passwords.Category> PasswordCategories { get; set; }
    public DbSet<Models.Passwords.Subcategory> PasswordSubcategories { get; set; }
    public DbSet<Models.WordLists.List> WordLists { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {

        /*** PASSWORDS ***/

        // Unique index on the password itself
        // TODO: Consider removing diacritics, etc
        builder.Entity<Models.Passwords.Password>().HasIndex(password => password.Name).IsUnique();

        // Set up the relationship between 1 password - N alternate spellings
        builder.Entity<Models.Passwords.AlternateSpelling>()
            .HasOne(alternate_spelling => alternate_spelling.Password)
            .WithMany(password => password.AlternateSpellings);

        // Set up the relationship between 1 password - N forbidden word
        builder.Entity<Models.Passwords.ForbiddenWord>()
            .HasOne(forbidden_word => forbidden_word.Password)
            .WithMany(password => password.ForbiddenWords);

        builder.Entity<Models.Passwords.TailoredWord>()
            .HasOne(tailored_word => tailored_word.Password)
            .WithMany(password => password.TailoredWords);

        /*** WORD LISTS ***/

        builder.Entity<Models.WordLists.ListList>()
            .HasOne(listlist => listlist.ParentList).WithMany(list => list.Lists);

        builder.Entity<Models.WordLists.ListList>()
            .HasOne(listlist => listlist.List);

        builder.Entity<Models.WordLists.ListWord>()
            .HasOne(listword => listword.ParentList).WithMany(list => list.Words);

        builder.Entity<Models.WordLists.List>().HasIndex(list => list.LegacyId).IsUnique();
        builder.Entity<Models.WordLists.List>().HasIndex(list => list.Name).IsUnique();
        builder.Entity<Models.WordLists.ListWord>().HasIndex(word => new { word.WordName, word.ParentListId });
        builder.Entity<Models.WordLists.ListList>().HasIndex(list => new { list.ListId, list.ParentListId });

        base.OnModelCreating(builder);

    }

}