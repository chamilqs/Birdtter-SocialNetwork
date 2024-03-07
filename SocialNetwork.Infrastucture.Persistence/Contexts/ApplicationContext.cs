using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Domain.Common;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ReplyComment> RepliesComment { get; set; }

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

            modelBuilder.Entity<Comment>()
                .ToTable("Comments");

            //modelBuilder.Entity<FriendRequest>()
            //    .ToTable("FriendRequests");

            modelBuilder.Entity<Friendship>()
                .ToTable("Friendships");

            modelBuilder.Entity<Post>()
                .ToTable("Posts");

            modelBuilder.Entity<ReplyComment>()
                .ToTable("RepliesComment");

            #endregion

            #region "Primary Keys"

            modelBuilder.Entity<Comment>()
                .HasKey(comment => comment.Id);

            modelBuilder.Entity<Friendship>()
                .HasKey(fs => fs.Id);

            modelBuilder.Entity<Post>()
                .HasKey(post => post.Id);

            modelBuilder.Entity<ReplyComment>()
                .HasKey(rc => rc.Id);

            #endregion

            #region "Relationships"

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
                //modelBuilder.Entity<SocialLink>(entity =>
                //{
                //    // Relaciones
                //    entity.HasOne(u => u.User)
                //          .WithOne(u => u.SocialLinks)
                //          .HasForeignKey<SocialLink>(u => u.UserId)
                //          .OnDelete(DeleteBehavior.Cascade);
                //});
                #endregion

            #endregion

            #region "Property configurations"

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

                #region FriendRequests(not used)
                //modelBuilder.Entity<FriendRequest>(entity => {

                //    // Propiedades
                //    entity.Property(fr => fr.SenderId)
                //          .IsRequired();
                //    entity.Property(fr => fr.RecieverId)
                //          .IsRequired();
                //    entity.Property(fr => fr.FriendRequestStatus)
                //          .IsRequired();

                //});
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
                    entity.Property(p => p.MediaVideo);

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

                #region SocialLinks (not used)
                //modelBuilder.Entity<SocialLink>(entity => {

                //    // Propiedades
                //    entity.Property(sl => sl.UserId)
                //          .IsRequired();

                //});
                #endregion

            #endregion

        }

    }
}
