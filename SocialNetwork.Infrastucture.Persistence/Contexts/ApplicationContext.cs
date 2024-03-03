using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Domain.Common;
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ReplyComment> RepliesComment { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach(var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FLUENT API

            #region "Table Names"

            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<Comment>()
                .ToTable("Comments");

            modelBuilder.Entity<FriendRequest>()
                .ToTable("FriendRequests");

            modelBuilder.Entity<Friendship>()
                .ToTable("Friendships");

            modelBuilder.Entity<Post>()
                .ToTable("Posts");

            modelBuilder.Entity<ReplyComment>()
                .ToTable("RepliesComment");

            modelBuilder.Entity<SocialLink>()
                .ToTable("SocialLinks");

            #endregion

            #region "Primary Keys"

            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);

            modelBuilder.Entity<Comment>()
                .HasKey(comment => comment.Id);

            modelBuilder.Entity<FriendRequest>()
                .HasKey(fr => fr.Id);

            modelBuilder.Entity<Friendship>()
                .HasKey(fs => fs.Id);

            modelBuilder.Entity<Post>()
                .HasKey(post => post.Id);

            modelBuilder.Entity<ReplyComment>()
                .HasKey(rc => rc.Id);

            modelBuilder.Entity<SocialLink>()
                .HasKey(sl => sl.Id);

            #endregion

            #region "Relationships"

                #region Users
                modelBuilder.Entity<User>(entity =>
                    {
                        // Relaciones
                        entity.HasMany(u => u.Friends)
                              .WithOne(u => u.User)
                              .HasForeignKey(g => g.UserId)
                              .OnDelete(DeleteBehavior.NoAction);
                        entity.HasMany(u => u.Comments)
                              .WithOne(u => u.User)
                              .HasForeignKey(u => u.UserId)
                              .OnDelete(DeleteBehavior.NoAction);
                        entity.HasMany(u => u.Posts)
                              .WithOne(u => u.User)
                              .HasForeignKey(u => u.UserId)
                              .OnDelete(DeleteBehavior.NoAction);
                        entity.HasMany(u => u.FriendRequests)
                              .WithOne(u => u.User)
                              .HasForeignKey(u => u.UserId)
                              .OnDelete(DeleteBehavior.NoAction);

                    });
                    #endregion

                #region Posts
                modelBuilder.Entity<Post>(entity =>
                {
                    // Relaciones
                    entity.HasMany(u => u.Comments)
                          .WithOne(u => u.Post)
                          .HasForeignKey(g => g.PostId)
                          .OnDelete(DeleteBehavior.Cascade);

                });
                #endregion

                #region Comments
                modelBuilder.Entity<Comment>(entity =>
                {
                    // Relaciones
                    entity.HasMany(u => u.Replies)
                          .WithOne(u => u.Comment)
                          .HasForeignKey(g => g.CommentId)
                          .OnDelete(DeleteBehavior.Cascade);

                });
                #endregion

                #region SocialLinks
                modelBuilder.Entity<SocialLink>(entity =>
                {
                    // Relaciones
                    entity.HasOne(u => u.User)
                          .WithOne(u => u.SocialLinks)
                          .HasForeignKey<SocialLink>(u => u.UserId)
                          .OnDelete(DeleteBehavior.Cascade);
                });
                #endregion

            #endregion

            #region "Property configurations"

                #region Users
            modelBuilder.Entity<User>(entity => {

                    // Propiedades
                    entity.Property(u => u.Name)
                          .IsRequired()
                          .HasMaxLength(150);
                    entity.Property(u => u.LastName)
                          .IsRequired()
                          .HasMaxLength(150);
                    entity.Property(u => u.Username)
                          .IsRequired()
                          .HasMaxLength(100);
                    entity.Property(u => u.Email)
                          .IsRequired();
                    entity.Property(u => u.EmailConfirmed)
                          .IsRequired();
                    entity.Property(u => u.Address)
                          .HasMaxLength(500);
                    entity.Property(u => u.Phone)
                          .IsRequired();
                    entity.Property(u => u.IsActive)
                          .IsRequired();
                    entity.Property(u => u.Password)
                          .IsRequired();

                });

            #endregion

                #region Comments
                modelBuilder.Entity<Comment>(entity => {

                        // Propiedades
                        entity.Property(c => c.Content)
                              .IsRequired()
                              .HasMaxLength(600);
                        entity.Property(c => c.PostId)
                              .IsRequired();
                        entity.Property(c => c.UserId)
                              .IsRequired();
                    });
            #endregion

                #region FriendRequests
                modelBuilder.Entity<FriendRequest>(entity => {

                    // Propiedades
                    entity.Property(fr => fr.SenderId)
                          .IsRequired();
                    entity.Property(fr => fr.UserId)
                          .IsRequired();
                    entity.Property(fr => fr.FriendRequestStatus)
                          .IsRequired();

                });
                #endregion

                #region Friendships
                modelBuilder.Entity<Friendship>(entity => {

                    // Propiedades
                    entity.Property(fs => fs.UserId)
                          .IsRequired();
                    entity.Property(fs => fs.FriendId)
                          .IsRequired();

                });
            #endregion

                #region Posts
                modelBuilder.Entity<Post>(entity => {

                    // Propiedades
                    entity.Property(p => p.Content)
                          .IsRequired()
                          .HasMaxLength(600);
                    entity.Property(p => p.UserId)
                          .IsRequired();
                    entity.Property(p => p.ImageUrl);

                });
            #endregion

                #region RepliesComment
                modelBuilder.Entity<ReplyComment>(entity => {

                    // Propiedades
                    entity.Property(rc => rc.Content)
                          .IsRequired()
                          .HasMaxLength(600);
                    entity.Property(rc => rc.CommentId)
                          .IsRequired();
                    entity.Property(u => u.UserId)
                          .IsRequired();

                });
            #endregion

                #region SocialLinks
            modelBuilder.Entity<SocialLink>(entity => {

                // Propiedades
                entity.Property(sl => sl.UserId)
                      .IsRequired();

            });
            #endregion

            #endregion

        }

    }
}
