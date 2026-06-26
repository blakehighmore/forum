using backend.Models.Auth;
using backend.Models.Categories;
using backend.Models.PostReactions;
using backend.Models.Posts;
using backend.Models.Profiles;
using backend.Models.Tags;
using backend.Models.Topics;
using backend.Models.Users;
using backend.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PostReaction> PostReactions => Set<PostReaction>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.HasPostgresExtension("pg_trgm");

        b.Entity<Post>()
            .HasIndex(p => p.Content)
            .HasMethod("gin")
            .HasOperators("gin_trgm_ops");


        b.Entity<RefreshToken>().HasIndex(rt => rt.Token).IsUnique();

        b.Entity<RefreshToken>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<User>().HasIndex(u => u.Username).IsUnique();


        b.Entity<Tag>()
            .Property(t => t.Color)
            .HasConversion(color => color.Value, value => Color.Create(value));

        b.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<Profile>(p => p.UserId);

        b.Entity<Category>()
            .HasMany(c => c.Topics)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Topic>()
            .HasMany(t => t.Posts)
            .WithOne(p => p.Topic)
            .HasForeignKey(p => p.TopicId)
            .OnDelete(DeleteBehavior.Cascade);


        b.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Topic>()
            .HasOne(t => t.Author)
            .WithMany()
            .HasForeignKey(t => t.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Topic>()
            .HasMany(t => t.Tags)
            .WithMany(t => t.Topics);


        b.Entity<PostReaction>()
            .HasOne(pr => pr.User)
            .WithMany()
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        b.Entity<PostReaction>()
            .HasOne(pr => pr.Post)
            .WithMany()
            .HasForeignKey(pr => pr.PostId);

        b.Entity<PostReaction>()
            .HasIndex(r => new { r.UserId, r.PostId })
            .IsUnique();
    }
}