using BotDatabase.Entry;
using Microsoft.EntityFrameworkCore;
using System;

namespace BotDatabase
{
    public class WorldQuestContext : DbContext
    {
        public DbSet<WorldQuestTask> WorldQuestTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=db\\world-quests.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Map(modelBuilder);
        }

        public static void Map(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new WorldQuestTask.Map());
        }
    }
}
