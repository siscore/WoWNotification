using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDatabase.Entry
{
    public class WorldQuestTask
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? QuestId { get; set; }
        public string QuestName { get; set; }

        public class Map : IEntityTypeConfiguration<WorldQuestTask>
        {
            public void Configure(EntityTypeBuilder<WorldQuestTask> builder)
            {
                builder.ToTable("WorldQuestTask");

                builder.HasKey(t => t.Id);

                builder.Property(p => p.Id).IsRequired();
                builder.Property(p => p.Id).ValueGeneratedOnAdd();

                builder.Property(p => p.UserId).IsRequired();
            }
        }
    }
}
