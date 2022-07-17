using Microsoft.EntityFrameworkCore;
using UsersAndGroupsAPI.Models;

namespace UsersAndGroupsAPI.Db
{
    public class UsersAndGroupsContext : DbContext
    {
        public DbSet<Group> Group => Set<Group>();
        public DbSet<User> User => Set<User>();
        
        public DbSet<UserGroup> UserGroup => Set<UserGroup>();
        public string DbPath { get; }

        public UsersAndGroupsContext(DbContextOptions<UsersAndGroupsContext> options)
            : base(options) {
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>()
               .HasIndex(p => new { p.UserId, p.GroupId }).IsUnique();
        }
    }
}
