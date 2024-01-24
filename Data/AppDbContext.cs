using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlankyBlankLibrary.Data;

public class AppDbContext : DbContext {

    public DbSet<Models.WordLists.List> WordLists { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {

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