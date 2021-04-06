using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotDatabase.Entry;
using BotDatabase.Interface;
using BotDatabase.Model;
using Microsoft.EntityFrameworkCore;

namespace BotDatabase.Service
{
    public class DbService : IDbService
    {
        private readonly WorldQuestContext _worldQuestContext;

        public DbService(WorldQuestContext worldQuestContext)
        {
            _worldQuestContext = worldQuestContext;
        }

        public async Task AddQuest(string userId, int questId)
        {
            await AddWorldQuestCore(userId, questId, null);
        }

        public async Task AddQuest(string userId, string questName)
        {
            await AddWorldQuestCore(userId, null, questName);
        }

        public async Task DeleteQuest(string userId, int questId)
        {
            await DeleteWorldQuestCore(userId, questId, null);
        }

        public async Task DeleteQuest(string userId, string questName)
        {
            await DeleteWorldQuestCore(userId, null, questName);
        }

        public async Task<List<WorldQuestTaskModel>> GetListByUser(string UserId)
        {
            var query = _worldQuestContext.Set<WorldQuestTask>()
                .Where(x => x.UserId.Equals(UserId));

            var result = await query.Select(x => new WorldQuestTaskModel
            {
                QuestId = x.QuestId,
                QuestName = x.QuestName
            }).ToListAsync();

            return result;
        }

        public async Task<List<UserWorldQuestModel>> GetUserToNotification(List<WorldQuestTaskModel> task)
        {
            var questsById = await _worldQuestContext.Set<WorldQuestTask>()
                .Where(x => task.Select(o => o.QuestId).Contains(x.QuestId))
                .Select(x => new UserWorldQuestModel
                {
                    UserId = x.UserId,
                    QuestId = x.QuestId,
                    QuestName = x.QuestName
                }).ToListAsync();

            var questsByName = await _worldQuestContext.Set<WorldQuestTask>()
                .Where(x => task.Select(o => o.QuestName).Contains(x.QuestName))
                .Select(x => new UserWorldQuestModel
                {
                    UserId = x.UserId,
                    QuestId = x.QuestId,
                    QuestName = x.QuestName
                }).ToListAsync();

            var result = questsById.Union(questsByName).ToList();

            result.ForEach(x => {
                if (string.IsNullOrEmpty(x.QuestName))
                {
                    x.QuestName = task
                        .Where(o => o.QuestId.Equals(x.QuestId))
                        .Select(x => x.QuestName)
                        .SingleOrDefault();
                }
                
                if (!x.QuestId.HasValue)
                {
                    x.QuestId = task
                        .Where(o => o.QuestName.Equals(x.QuestName))
                        .Select(x => x.QuestId)
                        .SingleOrDefault();
                }
            }); 

            return result;
        }

        private async Task AddWorldQuestCore(string userId, int? questId, string questName)
        {
            var query = _worldQuestContext.Set<WorldQuestTask>()
                    .Where(x => x.UserId.Equals(userId));

            if (questId.HasValue)
            {
                query = query
                    .Where(x => x.QuestId.Equals(questId));
            }

            if (!string.IsNullOrEmpty(questName))
            {
                query = query
                    .Where(x => x.QuestName.Equals(questName));
            }

            var entry = await query.SingleOrDefaultAsync();

            if (entry == null)
            {
                entry = new WorldQuestTask
                {
                    UserId = userId,
                    QuestId = questId
                };

                _worldQuestContext.Add(entry);
                await _worldQuestContext.SaveChangesAsync();
            }
        }

        private async Task DeleteWorldQuestCore(string userId, int? questId, string questName)
        {
            var query = _worldQuestContext.Set<WorldQuestTask>()
                    .Where(x => x.UserId.Equals(userId));

            if (questId.HasValue)
            {
                query = query
                    .Where(x => x.QuestId.Equals(questId));
            }

            if (!string.IsNullOrEmpty(questName))
            {
                query = query
                    .Where(x => x.QuestName.Equals(questName));
            }

            var entry = await query.SingleOrDefaultAsync();

            if (entry != null)
            {
                _worldQuestContext.Remove(entry);
                await _worldQuestContext.SaveChangesAsync();
            }
        }
    }
}
