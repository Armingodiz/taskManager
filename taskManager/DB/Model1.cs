using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace taskManager.DB
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=dataBase")
        {
        }

        public virtual DbSet<assignment> assignments { get; set; }
        public virtual DbSet<task> tasks { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<assignment>()
                .Property(e => e.taskName)
                .IsFixedLength();

            modelBuilder.Entity<task>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<task>()
                .Property(e => e.date)
                .IsFixedLength();

            modelBuilder.Entity<task>()
                .HasMany(e => e.assignments)
                .WithRequired(e => e.task)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<user>()
                .HasMany(e => e.assignments)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);
        }
    }
}
