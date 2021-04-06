using BotDatabase.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotDatabase.Interface
{
    public interface IDbService
    {
        Task AddQuest(string UserId, int questId);
        Task AddQuest(string UserId, string questName);
        Task DeleteQuest(string UserId, int questId);
        Task DeleteQuest(string UserId, string questName);
        Task<List<WorldQuestTaskModel>> GetListByUser(string UserId);
        Task<List<UserWorldQuestModel>> GetUserToNotification(List<WorldQuestTaskModel> task);
    }
}
